using System;
using System.Windows;
using System.Windows.Controls;
using Intelligent_AI_Platform.linker;
using Intelligent_AI_Platform.linker.manager;
using static Tensorflow.Binding;
using Intelligent_AI_Platform.pages.platform.app.GenericChat;
using PlatformLib.ui.framework;
using PlatformLib.ui.framework.activitymanager;
using PlatformLib.ui.framework.enumList;

namespace Intelligent_AI_Platform
{
    public partial class MainWindow: IActivityManager,IActivity,ISizeBox
    {
        public static double FixHeight;
        
        public MainWindow()
        {
            InitializeComponent();
            FixHeight = (SystemParameters.WindowCaptionButtonHeight + SystemParameters.ResizeFrameHorizontalBorderHeight + SystemParameters.WindowNonClientFrameThickness.Top + SystemParameters.WindowNonClientFrameThickness.Bottom);
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Init();
            if (_needInit)
            {
                _needInit = false;
                InitActivity();
                
            }
        }

        public void InitActivity()
        {
            Push(new ChatActivity(),PutStyle.FitChild);
        }

        public IPanel PutIn(IPanel panel, PutInStrategy putInStrategy = PutInStrategy.Push, PutStyle putStyle = PutStyle.FitChild)
        {
            switch (putInStrategy)
            {
                case PutInStrategy.Push:
                    return Push(panel,putStyle);
                case PutInStrategy.Replace:
                    return Replace(panel,putStyle);
                case PutInStrategy.ReplaceOrPush:
                    return UiRootFrame.Panel != null ? Replace(panel, putStyle) : Push(panel, putStyle);
                case PutInStrategy.ClearAllAndPush:
                    UiRootFrame.Clear();
                    return Push(panel, putStyle);
                default:
                    throw new ArgumentOutOfRangeException(nameof(putInStrategy), putInStrategy, null);
            }
        }

        public IPanel PutIn(IPanel panel, double width, double height, PutInStrategy putInStrategy = PutInStrategy.Push,
            PutStyle putStyle = PutStyle.FitChild)
        {
            if (Math.Abs(height - Height+FixHeight) > 1)
            {
             ((FrameworkElement)panel).Width = width;
             ((FrameworkElement)panel).Height = height;
             Linker.WindowsDispatcher.Dispatcher(panel);
             return null;// 返回值几乎不用
            }

            ((FrameworkElement)panel).Width = width;
            ((FrameworkElement)panel).Height = height;
            PutIn(panel, putInStrategy);
            return null;
        }

        
        public IPanel Push(IPanel panel, PutStyle putStyle)
        {
            if(putStyle == PutStyle.FitChild)
            {
                if (Math.Abs(((ISizeBox)panel).PreferredHeight - Height + FixHeight) > 1)
                {
                    Console.WriteLine(this + " " + "Dispatcher works Height " + ((ISizeBox)panel).PreferredHeight + " window height " + Height);
                    Linker.WindowsDispatcher.Dispatcher(panel);
                    return null;// 返回值几乎不用
                }
                SWidth = ((ISizeBox)panel).PreferredWidth;
                SMinHeight = ((ISizeBox)panel).SMinHeight + FixHeight;
                SMaxHeight = ((ISizeBox)panel).SMaxHeight + FixHeight;
                SMinWidth = ((ISizeBox)panel).SMinWidth;
                SMaxWidth = ((ISizeBox)panel).SMaxWidth;
            }

            // 自动会转移，通过WinSize
            SizeToContent = SizeToContent.Manual;
            var previousPanel = UiRootFrame.Push(panel, putStyle);
            WinSize_Changed();
            if (panel is Page page)
            {
                Title = page.Title;
            }
            return previousPanel;
        }
        public IPanel Replace(IPanel panel, PutStyle putStyle)
        {
            if (putStyle == PutStyle.FitChild)
            {
                if (Math.Abs(((ISizeBox)panel).SHeight - Height + FixHeight) > 1)
                {
                    PoP();
                    Linker.WindowsDispatcher.Dispatcher(panel);
                    return null;// 返回值几乎不用
                }
            }
            var previousPanel = UiRootFrame.Replace(panel, putStyle);
            if (putStyle == PutStyle.FitChild)
            {
                SWidth = ((ISizeBox)panel).PreferredWidth;
                SMinHeight = ((ISizeBox)panel).SMinHeight+FixHeight;
                SMaxHeight = ((ISizeBox)panel).SMaxHeight+FixHeight;
                SMinWidth = ((ISizeBox)panel).SMinWidth;
                SMaxWidth = ((ISizeBox)panel).SMaxWidth;
            }
            WinSize_Changed();
            if (panel is Page page)
            {
                Title = page.Title;
            }
            return previousPanel;
        }

        public IPanel PoP()
        {
            var previousPanel = UiRootFrame.PoP();
            WinSize_Changed();
            if (UiRootFrame.Panel is Page page)
            {
                Title = page.Title;
            }
            return previousPanel;
        }
        private void WinSize_Changed()
        {
            if (ActualHeight - FixHeight / 1.25 + 5 > 0)
            {
                // 这个效果和窗口触发事件一致，但是避免了负面效果，实际的窗口变化不会出现负面效果
                UiRootFrame.SHeight = ActualHeight - FixHeight/1.25+5;
                UiRootFrame.SWidth = ActualWidth;
            }
            else
            {
                // 这个分支实现的只是一个临时的中间效果，作用是赋给一个大小，使得窗口触发重排
                UiRootFrame.SHeight = SHeight;
                UiRootFrame.SWidth = SWidth;
            }
        }
        private void WinSize_Changed(object sender, SizeChangedEventArgs e)
        {
            UiRootFrame.SHeight = ActualHeight - FixHeight/1.25+5;
            UiRootFrame.SWidth = ActualWidth;
        }
        public IPanel BubbleUp(double width, double height)
        {
            return BubbleDown(width, height);
        }

        public IPanel BubbleDown(double width, double height)
        {
            var panel = UiRootFrame.GetPanel();
            Width = width;
            Height = height;
            return panel;
        }

        public void Init()
        {
            Linker.NavigatorManager.Register(NavigatorLabel.Root, this);
        }

        public void Destroy()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateView()
        {
            throw new System.NotImplementedException();
        }

        public double PreferredWidth
        {
            get => 800;
            set => throw new NotImplementedException();
        }
        public double PreferredHeight
        {
            get => 452;
            set => throw new NotImplementedException();
        }
        public double SWidth { get => Width; set => Width = value; }
        public double SHeight { get => Height; set => Height = value; }
        public double SMinWidth { get => MinWidth; set => MinWidth = value; }
        public double SMaxWidth { get => MaxWidth; set => MaxHeight = value; }
        public double SMinHeight { get => MinHeight; set => MinHeight = value; }
        public double SMaxHeight { get => MaxHeight; set => MaxHeight = value; }
        public double Flexibility { get => 1.0; set => throw new NotImplementedException(); }
        private static bool _needInit = true;

        
    }
}