using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Intelligent_AI_Platform.dataCenter;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.core.constants;
using OpenAI.core.models.chat.stream;
using OpenAI.instance;
using PlatformLib.ui.framework.ability;
using PlatformLib.ui.framework.layout;
using PlatformLib.ui.framework.state;

namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat.chatSession
{
    public partial class VerticalArrangedComponentManager : IHandRender, IDualInsertionStyle<FrameworkElement>
    {
        private List<InnerObject> _source = new List<InnerObject>();

        public class InnerObject
        {
            public FrameworkElement Element { set; get; }

            public long Time { set; get; }
        }

        public new ChatSession Parent { set; get; }
        private double _viewPosition;
        private double _renderHeight;
        private volatile bool _needScrollToEnd = true;
        private volatile bool _needMarkPosition;
        private volatile int _changeCount;
        private double LayoutMargin { set; get; }

        public VerticalArrangedComponentManager()
        {
            InitializeComponent();
        }

        public List<InnerObject> UserSource
        {
            set
            {
                _source = value;

                if (_changeCount > 0)
                {
                    _needScrollToEnd = false;
                }

                ReRender();
                // ReSharper disable once NonAtomicCompoundOperator
                _changeCount++;
            }
            get => _source;
        }

        public void Clear()
        {
            InnerCanvas.Children.Clear();
            _source.Clear();
            Render();
        }


        public void InsertBack(FrameworkElement frameworkElement, long time, bool end = false)
        {
            //var scrollViewer = FindVisualChild<ScrollViewer>(this);

            //获取当前滚动位置的偏移量
            //var offset = scrollViewer.ContentVerticalOffset;
            // _mutex.WaitOne();
            var innerObject = _source.FirstOrDefault(i => i.Time == time);
            if (innerObject == null)
            {
                _source.Add(new InnerObject() { Element = frameworkElement, Time = time });
            }
            else
            {
                innerObject.Element = frameworkElement;
            }

            Render();
            //await Task.Delay(20);
            //_mutex.ReleaseMutex();
            //_viewPosition = (int)(offset);
            //scrollViewer.ScrollToVerticalOffset(_viewPosition);
            // return 1;
        }


        public int InsertBack(List<FrameworkElement> list)
        {
            var scrollViewer = FindVisualChild<ScrollViewer>(this);

            //获取当前滚动位置的偏移量
            var offset = scrollViewer.ContentVerticalOffset;
            foreach (var element in list)
            {
                _source.Add(new InnerObject() { Element = element });
            }

            _viewPosition = (int)(offset);
            Render();
            scrollViewer.ScrollToVerticalOffset(_viewPosition);
            return list.Count;
        }

        public async Task PutNewTask(SessionContext sessionContext, long time,
            [Optional] CancellationTokenSource source)
        {
            var config = DataCenter.Configuration;
            Stream stream = null;
            var contentSb = new StringBuilder();
            StreamReader streamReader;
            try
            {
                var firstPrompt = DataCenter.Configuration.FirstPrompt;
                stream = await OpenAi.Instance.Chat.CreateStream2(model: config.Model,
                    sessionContext.Build(config.MaxTokens,firstPrompt),
                    // null,
                    temperature: config.Temperature,
                    maxTokens: config.MaxTokens);
                streamReader = new StreamReader(stream, Encoding.UTF8);
            }
            catch (Exception e)
            {
                var errorTalk = new Talk(Participant.Assistant, "")
                {
                    Error = "没有设置秘钥或远程服务器无法连接",
                    Additional = "-- 因为错误或异常而终止"
                };
                //sb.Append("没有设置秘钥或远程服务器无法连接"+ "-- 因为错误或异常而终止");
                var bubble = new Bubble("assistant", Width * 0.8,
                    ExpectedAlign.Left, errorTalk, true) { Vm = this };
                InsertBack(bubble, time);

                Parent.Session.Talks.Add(errorTalk);
                //Parent.SessionContext.Talks.Add(talk1);
                stream?.Close();
                return;
            }

            const int delay = 25;//25
            var talk = new Talk(Participant.Assistant, "");

            // api error
            var apiError = false;
            async Task GetData(OpenAiStreamChatCompletionModel model)
            {
                var r = model.Choices.FirstOrDefault();
                if (r != null)
                {
                    contentSb.Append(r.delta.Content);
                }

                await FindAndInsert();
                await Task.Delay(delay);
            }

            async Task Done()
            {
                await FindAndInsert(true);
                await Task.Delay(delay);
            }

            async Task GetDataError(string line)
            {
                //sb.Append(line);
                //error
                apiError = true;
                talk.Error = line;
                await FindAndInsert();
                await Task.Delay(delay);
            }

            // 黏着多个字符进行界面刷新
            const int beeNum = 4;//10
            var beeFlag = 0;
            

            async Task FindAndInsert(bool done = false, bool bee = true)
            {
                talk.Content = contentSb.ToString();
                if (!done)
                {
                    if (beeFlag < beeNum)
                    {
                        beeFlag++;
                        return;
                    }

                    beeFlag = 0;
                }

                await Task.Run(() =>
                {
                    var ele = _source.FirstOrDefault(I => I.Time == time);
                    if (ele != null)
                    {

                        Dispatcher.Invoke(() =>
                        {
                            var bubble = (Bubble)ele.Element;
                            bubble.RePaint(Width * 0.8, done);
                            Render();
                        });
                    }
                    else
                    {

                        Dispatcher.Invoke(() =>
                        {
                            var bubble = new Bubble("assistant", Width * 0.85,
                                ExpectedAlign.Left, talk, done) { Vm = this };
                            InsertBack(bubble, time);
                        });

                    }
                });

            }

            while (!streamReader.EndOfStream)
            {
                if (source is { IsCancellationRequested: true })
                {
                    break;
                }

                string line = null;
                try
                {
                    line = await streamReader.ReadLineAsync();
                    // Console.WriteLine(line);
                }
                catch (Exception e)
                {
                    break;
                }

                if (line.Trim() != "")
                {
                    if (line.StartsWith(OpenAiStrings.StreamResponseStart))
                    {
                        var data = line.Substring(6);
                        if (data.StartsWith(OpenAiStrings.StreamResponseEnd))
                        {
                            break;
                        }

                        var decoded = DecodeToMap(data);
                        var onData = OpenAiStreamChatCompletionModel.FromMap(decoded);
                        await GetData(onData);
                    }
                    else
                    {
                        await GetDataError(line);
                    }

                }
            }

            if (source is { IsCancellationRequested: true })
            {
                //sb.Append(ErrorText.Cancel);
                talk.Additional = ErrorText.Cancel;
            }

            streamReader.Close();
            stream.Close();
            await Done();
            Parent.Session.Talks.Add(talk);
            // 如果出现了api error 不添加到上下文
            if (!apiError)
            {
                Parent.SessionContext.Talks.Add(talk);
            }
        }


        public int InsertFront(List<FrameworkElement> list)
        {
            //获取ScrollViewer控件
            var scrollViewer = FindVisualChild<ScrollViewer>(this);

            //获取当前滚动位置的偏移量
            var offset = scrollViewer.ContentVerticalOffset;
            list.Reverse();
            foreach (var element in list)
            {
                _source.Insert(0, new InnerObject() { Element = element });
            }

            var ih = list.Sum(item => item.Height);
            _viewPosition = (int)(ih + offset);
            Render();
            scrollViewer.ScrollToVerticalOffset(_viewPosition);
            return list.Count;
        }

        private static TK FindVisualChild<TK>(DependencyObject parent) where TK : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is TK dependencyObject)
                    return dependencyObject;
                var foundChild = FindVisualChild<TK>(child);
                if (foundChild != null)
                    return foundChild;
            }

            return null;
        }


        public void Render()
        {
            LayoutMargin = 25;
            var renderWidth = Width;
            var renderPosition = 25d;
            foreach (var inner in _source)
            {
                var item = inner.Element;
                if (!(item is IExpectedPosition ep)) continue;
                switch (ep.ExpectedAlign)
                {
                    case ExpectedAlign.Left:
                        Canvas.SetLeft(item, 0d);
                        Canvas.SetTop(item, renderPosition);
                        //renderPosition += item.ActualHeight+LayoutMargin;
                        renderPosition += item.Height + LayoutMargin;
                        if (!InnerCanvas.Children.Contains(item))
                        {
                            InnerCanvas.Children.Add(item);
                        }

                        break;
                    case ExpectedAlign.Center:
                        Canvas.SetLeft(item, 1.0d * (renderWidth - item.Width) / 2);
                        Canvas.SetTop(item, renderPosition);
                        renderPosition += item.Height + LayoutMargin;
                        if (!InnerCanvas.Children.Contains(item))
                        {
                            InnerCanvas.Children.Add(item);
                        }

                        break;
                    case ExpectedAlign.Right:
                        Canvas.SetRight(item, 0);
                        Canvas.SetTop(item, renderPosition);
                        renderPosition += item.Height + LayoutMargin;
                        if (!InnerCanvas.Children.Contains(item))
                        {
                            InnerCanvas.Children.Add(item);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            InnerCanvas.Height = renderPosition;
            _renderHeight = renderPosition;
            if (_needScrollToEnd)
            {
                InnerScroll.ScrollToBottom();
                _needMarkPosition = true;
                // Console.WriteLine("Log Scroll End");
            }
            else
            {
                if (!(_viewPosition > 10)) return;
                InnerScroll.ScrollToVerticalOffset(_viewPosition);
                // Console.WriteLine("Log: Scroll to view at" + _viewPosition);
            }
        }


        public void ReRender()
        {
            InnerCanvas.Children.Clear();
            Render();
        }

        private void MessageListWidgetSizeChanged(object sender, SizeChangedEventArgs e)
        {
            
            var height = InnerCanvas.Height;
            if (height < Height)
            {
                InnerCanvas.Height = Height;
            }
            else if (height > _renderHeight)
            {
                InnerCanvas.Height = _renderHeight;
            }

            if (e.WidthChanged)//宽度改变才进行重绘
            {
                foreach (var innerObject in _source)
                {
                    ((Bubble)innerObject.Element).ReArrange(Width * 0.85);
                }

                Render();
            }
            //InnerScroll.ScrollToBottom();
            
            // _needMarkPosition = true;
            //Console.WriteLine("空间高度变化");
            //_needScrollToEnd = true;
        }

        private void ScrollViewScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_needMarkPosition)
            {
                _viewPosition = e.VerticalOffset;
                // Console.WriteLine("Log: view at"+_viewPosition);
            }
        }

        static Dictionary<string, dynamic> DecodeToMap(string responseBody)
        {
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(responseBody);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void ContextPaint()
        {
            foreach (var innerObject in _source)
            {
                var bubble = (Bubble)innerObject.Element;
                bubble.MarkAsContext = Parent.SessionContext.Talks.Contains(bubble.Talk);
            }
        }

        public void ContextPaint(Bubble bubble, bool isContext = true)
        {
            if (bubble.MarkAsContext == isContext)
            {
                return;
            }

            bubble.MarkAsContext = isContext;
        }

        public void ElementReArrange(Bubble bubble)
        {
            LayoutMargin = 25;
            var renderWidth = Width;
            var renderPosition = 25d;
            var startPartRender = false;
            foreach (var inner in _source)
            {
                var item = inner.Element;
                if (startPartRender == false)
                {
                    startPartRender = (Bubble)item == bubble;
                }
                if (!(item is IExpectedPosition ep)) continue;
                switch (ep.ExpectedAlign)
                {
                    case ExpectedAlign.Left:
                        if (startPartRender)
                        {
                            //Canvas.SetLeft(item, 0d);
                            Canvas.SetTop(item, renderPosition);
                        }

                        renderPosition += item.Height + LayoutMargin;

                        break;
                    case ExpectedAlign.Center:
                        if (startPartRender)
                        {
                            //Canvas.SetLeft(item, 1.0d * (renderWidth - item.Width) / 2);
                            Canvas.SetTop(item, renderPosition);
                        }

                        renderPosition += item.Height + LayoutMargin;

                        break;
                    case ExpectedAlign.Right:
                        if (startPartRender)
                        {
                            //Canvas.SetRight(item, 0);
                            Canvas.SetTop(item, renderPosition);
                        }

                        renderPosition += item.Height + LayoutMargin;

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            InnerCanvas.Height = renderPosition;
            _renderHeight = renderPosition;
        }
    }
}