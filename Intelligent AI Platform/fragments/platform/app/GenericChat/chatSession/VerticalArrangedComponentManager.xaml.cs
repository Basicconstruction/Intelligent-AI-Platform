using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Intelligent_AI_Platform.linker;
using Markdig.Helpers;
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

        public class  InnerObject
        {
            public FrameworkElement Element
            {
                set;
                get;
            }

            public long Time
            {
                set;
                get;
            }
        }

        public new ChatSession Parent
        {
            set;
            get;
        }
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
        private Mutex _mutex = new Mutex();

        public void InsertBack(FrameworkElement frameworkElement, long time,bool end = false)
        {
            //var scrollViewer = FindVisualChild<ScrollViewer>(this);

            //获取当前滚动位置的偏移量
            //var offset = scrollViewer.ContentVerticalOffset;
           // _mutex.WaitOne();
            var innerObject = _source.FirstOrDefault(i => i.Time == time);
            if (innerObject == null)
            {
                _source.Add(new InnerObject(){Element = frameworkElement,Time = time});
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
                _source.Add(new InnerObject(){Element = element});
            }
            
            _viewPosition = (int)(offset);
            Render();
            scrollViewer.ScrollToVerticalOffset(_viewPosition);
            return list.Count;
        }

        public async Task PutNewTask(SessionContext sessionContext, long time,[Optional] CancellationTokenSource source)
        {
            var config = Linker.Configuration;
            Stream stream = null;
            var sb = new StringBuilder();
            StreamReader streamReader = null;
            try
            {
                stream = await OpenAi.Instance.Chat.CreateStream2(model: config.Model,
                    sessionContext.Build(config.MaxTokens),
                    // null,
                    temperature: config.Temperature,
                    maxTokens: config.MaxTokens);
                streamReader = new StreamReader(stream, Encoding.UTF8);
            }
            catch (Exception e)
            {
                sb.Append("没有设置秘钥或远程服务器无法连接"+e + "-- 因为错误或异常而终止");
                var bubble = new Bubble("assistant", Width * 0.8,
                    ExpectedAlign.Left, sb.ToString(),true);
                InsertBack(bubble, time);
                var talk1 = new Talk(Participant.Assistant, sb.ToString()) { Time= DateTimeOffset.Now.ToUnixTimeMilliseconds() };
                Parent.Session.Talks.Add(talk1);
                Parent.SessionContext.Talks.Add(talk1);
                stream?.Close();
                return;
            }
            async Task GetData(OpenAiStreamChatCompletionModel model)
            {
                var r = model.Choices.FirstOrDefault();
                if (r != null)
                {
                    sb.Append(r.delta.Content);
                }
                FindAndInsert();
                await Task.Delay(2);
            }
            async Task Done()
            {
                FindAndInsert(true);
                await Task.Delay(2);
            }
            async Task GetDataError(string line)
            {
                sb.Append(line);
                FindAndInsert();
                await Task.Delay(2);
            }

            void FindAndInsert(bool done=false)
            {
                var t1 = sb.ToString();
                var ele = _source.FirstOrDefault(I => I.Time == time);
                if (ele != null)
                {
                    var bubble = (Bubble)ele.Element;
                    bubble.RePaint(t1,Width*0.8,done);
                    Render();
                }
                else
                {
                    var bubble = new Bubble("assistant", Width * 0.8,
                        ExpectedAlign.Left, t1,done);
                    InsertBack(bubble, time);
                        
                }
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
                sb.Append("\n  " +
                          "-------  通过令牌取消");
            }
            streamReader.Close();
            stream.Close();
            await Done();
            var talk = new Talk(Participant.Assistant, sb.ToString()) { Time= DateTimeOffset.Now.ToUnixTimeMilliseconds() };
            Parent.Session.Talks.Add(talk);
            Parent.SessionContext.Talks.Add(talk);
            
         }
        [Obsolete]
        public async Task PutTask(SessionContext sessionContext, long time)
        {
            var config = Linker.Configuration;
            var stream = await OpenAi.Instance.Chat.CreateStream(
                model: config.Model,
                sessionContext.Build(config.MaxTokens),
                temperature: config.Temperature,
                maxTokens: config.MaxTokens
            );
            var sb = new StringBuilder();
            stream.Subscribe(
                data =>
                {
                    var first = data.Choices.FirstOrDefault();
                    // Console.WriteLine(first.index);
                    // Console.WriteLine(first.delta.Content);
                    if (first != null)
                    {
                        var r = first.delta.Content;
                        sb.Append(r);
                    }

                    var t1 = sb.ToString();
                    var ele = _source.FirstOrDefault(I => I.Time == time);
                    if (ele != null)
                    {
                        var bubble = (Bubble)ele.Element;
                        bubble.RePaint(t1,Width*0.8);
                        Render();
                    }
                    else
                    {
                        var bubble = new Bubble("assistant", Width * 0.8,
                            ExpectedAlign.Left, t1);
                        // var bubble = new Bubble(typeof(MarkdownLabel), new object[] { t1 },
                        //     "assistant",Width*0.8,ExpectedAlign.Left);
                        InsertBack(bubble, time);
                        
                    }
                    
                    Console.WriteLine("hello " + t1);
                }, (e) =>
                {
                    sb.Append(e.Message);
                    var t0 = sb.ToString();
                    var ele = _source.FirstOrDefault(I => I.Time == time);
                    if (ele != null)
                    {
                        var bubble = (Bubble)ele.Element;
                        bubble.RePaint(t0,Width*0.8);
                        Render();
                    }
                    else
                    {
                        // var bubble = new Bubble("assistant", Width * 0.8,
                        //     ExpectedAlign.Left, t0);
                        var bubble = new Bubble(typeof(MarkdownLabel), new object[] { t0 },
                            "assistant",Width*0.8,ExpectedAlign.Left);
                        InsertBack(bubble,time);
                        
                    }

                    
                    Console.WriteLine(e.ToString());
                    //await Task.Delay(20);
                }, () =>
                {
                    var text = sb.ToString();
                    var ele = _source.FirstOrDefault(I => I.Time == time);
                    if (ele != null)
                    {
                        var bubble = (Bubble)ele.Element;
                        bubble.RePaint(text,Width*0.8,true);
                        var talk = new OpenAI.Talk(OpenAI.Participant.Assistant, text);
                        Parent.Session.Talks.Add(talk);
                        Parent.SessionContext.Talks.Add(talk);
                        Render();
                        Console.WriteLine("done");
                    }
                }
                );
        }
        [Obsolete]
        public async Task PutTask(string content, long time)
        {
            const string openAiApiKey = "";

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content =
                            "You are ChatGPT, a large language model trained by OpenAI. Follow the user's instructions carefully. Respond using markdown.并且使用中文回答。"
                    },
                    new { role = "user", content = content },
                },
                stream = true,
                max_tokens = 3000,
            };
            var httpClient = new HttpClient();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8,
                "application/json");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai-sb.com/v1/chat/completions");
            request.Content = jsonContent;

            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            var responseBodyStream = await response.Content.ReadAsStreamAsync();
            var streamReader = new StreamReader(responseBodyStream, Encoding.UTF8);
            IWatcher watcher = new LineWater();
            var stringBuilder = new StringBuilder();
            while (!streamReader.EndOfStream)
            {
                var aline = await streamReader.ReadLineAsync();

                if (!watcher.NeedIgnore(aline))
                {
                    stringBuilder.Append(ExtractContent(aline));
                    var text = stringBuilder.ToString();
                    var ele = _source.FirstOrDefault(I => I.Time == time);
                    if (ele != null)
                    {
                        var bubble = (Bubble)ele.Element;
                        bubble.RePaint(text,Width*0.8);
                        Render();
                    }
                    else
                    {
                        var bubble = new Bubble(typeof(MarkdownLabel), new object[] { text },
                        "assistant",Width*0.8,ExpectedAlign.Left);
                        InsertBack(bubble, time);
                        
                    }
                    await Task.Delay(20);
                    Console.WriteLine(ExtractContent(aline));
                }
            }
            //done
            {
                var text = stringBuilder.ToString();
                var ele = _source.FirstOrDefault(I => I.Time == time);
                if (ele != null)
                {
                    var bubble = (Bubble)ele.Element;
                    bubble.RePaint(text,Width*0.8,true);
                    var talk = new OpenAI.Talk(OpenAI.Participant.Assistant, text);
                    Parent.Session.Talks.Add(talk);
                    Parent.SessionContext.Talks.Add(talk);
                    Render();
                }
            }
            GC.Collect();
            GC.WaitForFullGCComplete();
        }
        private static string ExtractContent(string line)
        {
            var regex = new Regex("\"content\":\"([^\"]+)\"");
            var match = regex.Match(line);

            if (match.Success && match.Groups.Count > 1)
            {
                return match.Groups[1].Value;
            }

            return "";
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
                _source.Insert(0,new InnerObject(){Element = element});
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
            //var renderWidth = ActualWidth;
            var renderWidth = Width;
            var renderPosition = 25d;
            //Console.WriteLine("FFFF count: "+_source.Count);
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
                    case ExpectedAlign.Default:
                        break;
                    case ExpectedAlign.Start:
                        break;
                    case ExpectedAlign.End:
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

            InnerScroll.ScrollToBottom();
            foreach (var innerObject in _source)
            {
                ((Bubble)innerObject.Element).ReArrange(Width*0.8);
            }
            Render();
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
                // 未验证
                return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(responseBody);
            }
            catch (Exception e)
            {
                return null;
                throw new Exception(e.ToString());
            }
        }
    }
}