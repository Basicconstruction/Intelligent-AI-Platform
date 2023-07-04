using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PlatformLib.ui.framework.layout;

namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat.chatSession
{
    public sealed class Bubble:UserControl,IExpectedPosition
    {
        private const double OtherWidgetHeight = 30;

        private const double OtherWidgetWidth = 10;
        private const double BorderFix = 14;
        private const double BorderMarginFix = 60;
        public ExpectedAlign ExpectedAlign { get; set; }
        private readonly Border _border;
        private readonly Label _label;
        private readonly InnerArgs _innerArg = new InnerArgs();

        private class InnerArgs
        {
            public string Content;
            public double DesignWidth;
            public string Name;
        }

        public void UseMarkdown(bool useMarkdown)
        {
            _element.UseMarkdown(useMarkdown);
        }

        private readonly MarkdownLabel _element = null;

        public Bubble(string name,double preferWidth,ExpectedAlign align,string content,bool final = true)
        {
            ExpectedAlign = align;
            preferWidth -= BorderMarginFix;
            _innerArg.DesignWidth = preferWidth;
            _innerArg.Name = name;
            _innerArg.Content = content;
            _element = new MarkdownLabel(_innerArg.Content, _innerArg.DesignWidth - OtherWidgetWidth);
            _element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            _element.Arrange(new Rect(0, 0, _element.DesiredSize.Width, _element.DesiredSize.Height));
            var width = _element.ActualWidth;
            var height = _element.ActualHeight;
            _border = new Border
            {
                //Background = new SolidColorBrush(Colors.Peru),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(5),
                BorderBrush = new SolidColorBrush(Colors.Blue),
                Child = _element,
                Width = width + 20,
                Height = height + 20
            };
            if (final)
            {
                _border.Padding = new Thickness(6);
            }
            else
            {
                _border.Padding = new Thickness(10);
            }
            var canvas = new Canvas();
            _label = new Label
            {
                Content = name,
                Width = 100,
                Height = 30,
            };  
            if (align == ExpectedAlign.Left)
            {
                Width = _border.Width + OtherWidgetWidth;// 14 为字体修正
                Canvas.SetLeft(_border, OtherWidgetWidth+ BorderMarginFix);
                Canvas.SetLeft(_label,0);
            }
            else
            {
                Width = _border.Width + OtherWidgetWidth + BorderFix;
                Canvas.SetRight(_border, OtherWidgetWidth+BorderFix+ BorderMarginFix);
                Canvas.SetRight(_label,0);
            }
            Canvas.SetTop(_label,0);
            Height = _border.Height + OtherWidgetHeight;
            AddChild(canvas);
            Canvas.SetBottom(_border, 0);
            canvas.Children.Add(_border);
            canvas.Children.Add(_label);
        }
        [Obsolete]
        public Bubble(Type type,object[] constructArgs,string name,double preferWidth,ExpectedAlign align)
        {
            ExpectedAlign = align;
            preferWidth -= BorderMarginFix;
            _innerArg.DesignWidth = preferWidth;
            _innerArg.Content = (string)constructArgs[0];
            _innerArg.Name = name;
            // var args = new List<object>();
            // args.AddRange(constructArgs);
            
            // args.Add(preferWidth - OtherWidgetWidth);
            
            // var element = (FrameworkElement)Activator.CreateInstance(type, args.ToArray());
            _element = new MarkdownLabel(_innerArg.Content, _innerArg.DesignWidth - OtherWidgetWidth);
            _element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            _element.Arrange(new Rect(0, 0, _element.DesiredSize.Width, _element.DesiredSize.Height));
            var width = _element.ActualWidth;
            var height = _element.ActualHeight;
            _border = new Border
            {
                //Background = new SolidColorBrush(Colors.Peru),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(10),
                BorderBrush = new SolidColorBrush(Colors.Blue),
                Child = _element,
                Width = width + 20,
                Height = height + 20
            };
            var canvas = new Canvas();
            _label = new Label
            {
                Content = name,
                Width = 100,
                Height = 30,
            };  
            if (align == ExpectedAlign.Left)
            {
                Width = _border.Width + OtherWidgetWidth;// 14 为字体修正
                Canvas.SetLeft(_border, OtherWidgetWidth+ BorderMarginFix);
                Canvas.SetLeft(_label,0);
            }
            else
            {
                Width = _border.Width + OtherWidgetWidth + BorderFix;
                Canvas.SetRight(_border, OtherWidgetWidth+BorderFix+ BorderMarginFix);
                Canvas.SetRight(_label,0);
            }
            Canvas.SetTop(_label,0);
            Height = _border.Height + OtherWidgetHeight;
            AddChild(canvas);
            Canvas.SetBottom(_border, 0);
            canvas.Children.Add(_border);
            canvas.Children.Add(_label);
        }

        public void RePaint(string content,double designWidth,bool done = false)
        {
            _innerArg.Content = content;
            _innerArg.DesignWidth = designWidth;
            Repaint(done);
        }

        public void ReArrange(double designWidth,bool done = true)
        {
            _innerArg.DesignWidth = designWidth;
            Repaint(done);
        }
        private void Repaint(bool done = false)
        {
            _element.RePaint(_innerArg.Content, _innerArg.DesignWidth);
            _element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            _element.Arrange(new Rect(0, 0, _element.DesiredSize.Width, _element.DesiredSize.Height));
            var width = _element.ActualWidth;
            var height = _element.ActualHeight;
            _border.Width = width + 20;
            _border.Height = height + 20;
            if (done)
            {
                _border.Padding = new Thickness(6);
            }
            else
            {
                _border.Padding = new Thickness(10);
            }
            _label.Content = _innerArg.Name;
            if (ExpectedAlign == ExpectedAlign.Left)
            {
                Width = _border.Width + OtherWidgetWidth;// 14 为字体修正
                Canvas.SetLeft(_border, OtherWidgetWidth + BorderMarginFix);
                Canvas.SetLeft(_label, 0);
            }
            else
            {
                Width = _border.Width + OtherWidgetWidth + BorderFix;
                Canvas.SetRight(_border, OtherWidgetWidth + BorderFix + BorderMarginFix);
                Canvas.SetRight(_label, 0);
            }
            Canvas.SetTop(_label, 0);
            Height = _border.Height + OtherWidgetHeight;
            Canvas.SetBottom(_border, 0);
            // Console.WriteLine("repaint"+_border.Padding);
            // Console.WriteLine("repaint width: "+width+" height "+height);
        }
    }
}