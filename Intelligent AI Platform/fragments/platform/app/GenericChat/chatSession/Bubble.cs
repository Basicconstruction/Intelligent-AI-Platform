using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Intelligent_AI_Platform.linker;
using OpenAI;
using PlatformLib.ui.framework.layout;
using Label = System.Windows.Controls.Label;
using UserControl = System.Windows.Controls.UserControl;

namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat.chatSession
{
    public sealed class Bubble:UserControl,IExpectedPosition
    {
        private const double OtherWidgetHeight = 30;
        private const double OtherWidgetWidth = 10;
        private const double BorderFix = 14;
        private const double BorderMarginFix = 60;
        private double BorderUsage = 20;
        public ExpectedAlign ExpectedAlign { get; set; }
        private readonly Border _border;
        private readonly Label _label;
        private readonly InnerArgs _innerArg = new InnerArgs();
        private VerticalArrangedComponentManager _verticalArrangedComponentManager;
        public VerticalArrangedComponentManager Vm
        {
            get => _verticalArrangedComponentManager;
            set
            {
                _verticalArrangedComponentManager = value;
                InitContext();
            }
        }

        private ChatSession ChatSession => Vm?.Parent;
        private Session Session => Vm?.Parent.Session;
        private SessionContext SessionContext => Vm?.Parent.SessionContext;
        private class InnerArgs
        {
            public string Content;
            public double DesignWidth;
            public string Name;
        }

        public void InitContext()
        {
            var menu = new ContextMenu();
            var item1 = new MenuItem()
            {
                Header = "从此项开始作为上下文",
            };
            var item2 = new MenuItem()
            {
                Header = "将此项添加到上下文"
            };
            var item3 = new MenuItem()
            {
                Header = "将此项移出上下文"
            };
            var item4 = new MenuItem()
            {
                Header = "显示原文",
            };
            var item5 = new MenuItem()
            {
                Header = "复制原文"
            };
            var item6 = new MenuItem()
            {
                Header = "使用Markdown格式显示"
            };
            item1.Click += (sender, args) =>
            {
                SessionContext.Clear();
                SessionContext.AddContext(Session.BuildContextFrom(Talk));
                Vm.ContextPaint();
            };
            item2.Click += (sender, args) =>
            {
                SessionContext.AddContext(Talk);
                Vm.ContextPaint(this,true);
            };
            item3.Click += (sender, args) =>
            {
                SessionContext.RemoveContext(Talk);
                Vm.ContextPaint(this,false);
            };
            item4.Click += (sender, args) =>
            {
                UseMarkdown(false);
                Vm.ElementReArrange(this);
            };
            item5.Click += (sender, args) =>
            {
                Clipboard.SetText(Talk.Content);
            };
            item6.Click += (sender, args) =>
            {
                UseMarkdown(true);
                Vm.ElementReArrange(this);
            };
            menu.Items.Add(item5);
            menu.Items.Add(item4);
            menu.Items.Add(item6);
            menu.Items.Add(item1);
            menu.Items.Add(item2);
            menu.Items.Add(item3);
            ContextMenu = menu;
        }

        public Talk Talk
        {
            get;
            set;
        }

        private bool _markAsContext = false;

        public bool MarkAsContext
        {
            get => _markAsContext;
            set
            {
                if(_markAsContext==value)return;
                _markAsContext = value;
                if (value)
                {
                    _border.BorderThickness = new Thickness(4);
                    _border.BorderBrush = Linker.ContextBrush;
                }
                else
                {
                    _border.BorderThickness = new Thickness(2);
                    _border.BorderBrush = Linker.NotContextBrush;
                }
                Repaint(true);
            }
        }

        private void UseMarkdown(bool useMarkdown)
        {
            if (_element.UseMarkdownFlag != useMarkdown)
            {
                _element.UseMarkdown(useMarkdown);
                Repaint(true);
            }
        }
        private Canvas InnerCanvas { get; set; }

        private readonly MarkdownLabel _element = null;

        public Bubble(string name,double preferWidth,ExpectedAlign align,Talk talk,bool final = true)
        {
            ExpectedAlign = align;
            preferWidth -= BorderMarginFix;
            _innerArg.DesignWidth = preferWidth;
            _innerArg.Name = name;
            Talk = talk;
            _innerArg.Content = talk.Content+talk.Error+talk.Additional;
            BorderUsage = ((final?8:0)) * 2;
            _element = new MarkdownLabel(_innerArg.Content, _innerArg.DesignWidth - OtherWidgetWidth - BorderUsage-BorderMarginFix);
            _element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            _element.Arrange(new Rect(0, 0, _element.DesiredSize.Width, _element.DesiredSize.Height));
            var width = _element.ActualWidth;
            var height = _element.ActualHeight;
            
            _border = new Border
            {
                CornerRadius = new CornerRadius(5),
                BorderBrush = new SolidColorBrush(Colors.Blue),
                Child = _element,
                Width = width + BorderUsage,
                Height = height + BorderUsage,
            };
            if (final)
            {
                _border.Padding = new Thickness(6);
                _border.BorderThickness = new Thickness(2);
            }
            else
            {
                _border.Padding = new Thickness(0);
            }
            InnerCanvas = new Canvas();
            _label = new Label
            {
                Content = name,
                Width = 100,
                Height = 30,
            };  
            if (align == ExpectedAlign.Left)
            {
                Width = _border.Width + OtherWidgetWidth+BorderMarginFix;// 14 为字体修正
                Canvas.SetLeft(_border, OtherWidgetWidth+ BorderMarginFix);
                Canvas.SetLeft(_label,0);
            }
            else
            {
                Width = _border.Width + OtherWidgetWidth + BorderFix+BorderMarginFix;
                Canvas.SetRight(_border, OtherWidgetWidth+BorderFix+ BorderMarginFix);
                Canvas.SetRight(_label,0);
            }
            Canvas.SetTop(_label,0);
            Height = _border.Height + OtherWidgetHeight;
            AddChild(InnerCanvas);
            Canvas.SetTop(_border, OtherWidgetHeight);
            InnerCanvas.Children.Add(_border);
            InnerCanvas.Children.Add(_label);
            //InitContext();
        }
        
        public void RePaint(double designWidth,bool done = false)
        {
            _innerArg.Content = Talk.Content+Talk.Error+Talk.Additional;
            _innerArg.DesignWidth = designWidth;
            Repaint(done);
        }

        public void ReArrange(double designWidth,bool done = true)
        {
            if (Math.Abs(designWidth - _innerArg.DesignWidth) < 0.02)
            {
                return;
            }
            _innerArg.DesignWidth = designWidth;
            Repaint(done);
        }
        private void Repaint(bool done = false)
        {
            var prefer = 0.0;
            if (_border.BorderThickness.Left > 0)
            {
                prefer = _border.BorderThickness.Left;
            }
            else
            {
                prefer = 2;// default
            }
            BorderUsage = ((done?6+prefer:0))*2;
            _element.RePaint(_innerArg.Content, _innerArg.DesignWidth-OtherWidgetWidth-BorderUsage-BorderMarginFix);
            _element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            _element.Arrange(new Rect(0, 0, _element.DesiredSize.Width, _element.DesiredSize.Height));
            var width = _element.ActualWidth;
            var height = _element.ActualHeight;
            
            _border.Width = width + BorderUsage;
            _border.Height = height + BorderUsage;
            //_border.Padding = new Thickness(6);
            if (done)
            {
                _border.Padding = new Thickness(6);
                if (!(prefer > 0))
                {
                    _border.BorderThickness = new Thickness(2);
                }
            }
            else
            {
                _border.Padding = new Thickness(0);
            }
            _label.Content = _innerArg.Name;
            if (ExpectedAlign == ExpectedAlign.Left)
            {
                Width = _border.Width + OtherWidgetWidth+BorderMarginFix;// 14 为字体修正
                // Canvas.SetLeft(_border, OtherWidgetWidth + BorderMarginFix);
                // Canvas.SetLeft(_label, 0);
            }
            else
            {
                Width = _border.Width + OtherWidgetWidth + BorderFix+BorderMarginFix;
                // Canvas.SetRight(_border, OtherWidgetWidth + BorderFix + BorderMarginFix);
                // Canvas.SetRight(_label, 0);
            }
            Height = _border.Height + OtherWidgetHeight;
            // InnerCanvas.Children.Remove(_border);
            // InnerCanvas.Children.Add(_border);
            //InnerCanvas.UpdateLayout();
            // Canvas.SetBottom(_border, 0);
            // Console.WriteLine($"Height {Height} Border Height {_border.Height} Border Padding {_border.Padding.Top}" +
            //                   $"border thickness {_border.BorderThickness.Top} element height {_element.Height}" +
            //                   $"element actualheight {_element.ActualHeight} element ");
        }
    }
}