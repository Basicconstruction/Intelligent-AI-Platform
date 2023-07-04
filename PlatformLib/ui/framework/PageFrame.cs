using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PlatformLib.ui.framework.activitymanager;
using PlatformLib.ui.framework.enumList;

namespace PlatformLib.ui.framework
{
    public class PageFrame: Frame,IPageRef,IActivityManager,ISizeBox
    {
        private readonly List<IPanel> _panels = new List<IPanel>();

        public void Clear()
        {
            _panels.Clear();
        }

        public double SWidth { get => Width; 
            set
            {
                Width = value;
                switch (Panel)
                {
                    // ReSharper disable SuspiciousTypeConversion.Global
                    case ISizeBox box:
                        box.SWidth = Width;
                        box.SHeight = Height;
                        break;
                    case FrameworkElement frameworkElement:
                        frameworkElement.Width = Width;
                        frameworkElement.Height = Height;
                        break;
                }
                //Console.WriteLine("sync ");
            }
        }
        public double SHeight { 
            get => Height; 
            set {
                Height = value; 
                switch (Panel)
                {
                    // ReSharper disable SuspiciousTypeConversion.Global
                    case ISizeBox box:
                        box.SWidth = Width;
                        box.SHeight = Height;
                        break;
                    case FrameworkElement frameworkElement:
                        frameworkElement.Width = Width;
                        frameworkElement.Height = Height;
                        break;
                }
            }
        }
        public double PreferredWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double PreferredHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double SMinWidth { get => MinWidth; set => MinWidth = value; }
        public double SMaxWidth { get => MaxWidth; set =>  MaxWidth = value; }
        public double SMinHeight { get => MinHeight; set => MinHeight= value; }
        public double SMaxHeight { get => MaxHeight; set => MaxHeight = value; }
        public double Flexibility { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IPanel Panel
        {
            get;
            // 私有属性
            private set;
        }

        public IPanel GetPanel() => Panel;
        public List<IPanel> GetPanels() => _panels;

        private bool EnhancedNavigate(object content,bool fitChild)
        {
            // 代码和人有一个能跑就行，使用Push不会报错
            if (content is ISizeBox box)
            {
                if (fitChild)
                {
                    Width = box.PreferredWidth;
                    Height = box.PreferredHeight;
                    MinWidth = box.SMinWidth;
                    MinHeight = box.SMinHeight;
                    MaxWidth = box.SMaxWidth;
                    MaxHeight = box.SMaxHeight;
                }
                else
                {
                    switch (box)
                    {
                        case FrameworkElement page:
                            if (Math.Abs(ActualHeight) > 1)
                            {
                                page.Width = ActualWidth;
                                page.Height = ActualHeight;
                            }
                            else
                            {
                                page.Width = Width;
                                page.Height = Height;
                            }
                            
                            page.MinWidth = MinWidth;
                            page.MinHeight = MinHeight;
                            page.MaxWidth = MaxWidth;
                            page.MaxHeight = MaxHeight;
                            
                            
                            // MinWidth = page.MinWidth;
                            // MinHeight = page.MinHeight;
                            // MaxWidth = page.MaxWidth;
                            // MaxHeight = page.MaxHeight;
                            //Console.WriteLine("NNN Height : "+page.Height);
                            break;
                    }
                }
                

            }
            Dispatcher.BeginInvoke(new Action(() => { }));
            //Console.WriteLine("" + this + this.GetHashCode() + "Frame changed " + Height + " " + Width+ " A" + ActualHeight + " " + ActualWidth);

            return Navigate(content);
        }
        public void InitActivity()
        {
            
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
                    return Panel != null ? Replace(panel, putStyle) : Push(panel, putStyle);
                case PutInStrategy.ClearAllAndPush:
                    _panels.Clear();
                    return Push(panel, putStyle);
                default:
                    throw new ArgumentOutOfRangeException(nameof(putInStrategy), putInStrategy, null);
            }

        }

        public IPanel PutIn(IPanel panel, double width, double height, PutInStrategy putInStrategy = PutInStrategy.Push,
            PutStyle putStyle = PutStyle.FitChild)
        {
            if (!(panel is FrameworkElement frameworkElement)) return PutIn(panel, putInStrategy, putStyle);
            frameworkElement.Width = width;
            frameworkElement.Height = height;
            return PutIn(panel, putInStrategy, putStyle);
        }
        /*
         * 返回值是压入之前的 页面
         * *
         */ 
        public IPanel Push(IPanel panel,PutStyle putStyle)
        {
            _panels.Add(panel);
            Panel = panel;
            EnhancedNavigate(panel,putStyle==PutStyle.FitChild);
            return _panels.Count-2 >= 0 ? _panels[_panels.Count - 2] : null;
            
        }
        /*
         * 返回值是弹出之前的 页面
         * *
         */ 
        public IPanel PoP()
        {
            if (_panels.Count < 2) throw new Exception("错误的栈操作，没有如此多的 Activity");
            var panel = _panels[_panels.Count - 1];
            _panels.RemoveAt(_panels.Count-1);
            Panel = _panels[_panels.Count - 1];
            EnhancedNavigate(Panel, false);
            return panel;

        }
        /*
         * 返回值是替换之前的 页面
         * *
         */ 
        public IPanel Replace(IPanel panel,PutStyle putStyle)
        {
            if (Panel == null)
            {
                throw new Exception("请使用Push");
            }
            var activity = Panel as IActivity;
            _panels.RemoveAt(_panels.Count-1);
            _panels.Add(panel);
            Panel = panel;
            EnhancedNavigate(panel,putStyle==PutStyle.FitChild);
            return activity;

        }
        
        // 没有进行多次传递，仅仅单向传递，后续可以进行再设计
        // 从内向外进行传递，从 panel 传递到Frame
        public IPanel BubbleUp(double width, double height)
        {
            ((FrameworkElement)Panel).Width = Width;
            ((FrameworkElement)Panel).Height = Height;
            Width = width; 
            Height = height;
            return Panel;
        }
        public IPanel BubbleDown(double width, double height)
        {
            Width = width;
            Height = height;
            ((FrameworkElement)Panel).Width = Width;
            ((FrameworkElement)Panel).Height = Height;
            return Panel;
        }
    }
}