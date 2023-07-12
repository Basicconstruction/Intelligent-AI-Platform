using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Intelligent_AI_Platform.fragments.platform.app.GenericChat.item;

namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat.chatSession
{
    public delegate void Send(string msg);
    public partial class InputBox
    {
        public event Send Send;

        public new ChatSession Parent
        {
            set;
            get;
        }
        public InputBox()
        {
            InitializeComponent();
            SendBt.Background = new SolidColorBrush(Colors.LightBlue);
            SendBt.BorderBrush = new SolidColorBrush(Colors.LightBlue);
            SendBtBorder.Background = new SolidColorBrush(Colors.LightBlue);
            SendBtBorder.BorderBrush = new SolidColorBrush(Colors.LightBlue);
            TextArea.PlaceHolder = "请输入你的问题。";
        }

        private Stop _stop;

        public void RemoveStop()
        {
            if (InnerCanvas.Children.Contains(_stop))
            {
                _stop.Dispose();
                InnerCanvas.Children.Remove(_stop);
                _stop = null;
            }
            
        }

        public void AddCancelToken(CancellationTokenSource source)
        {
            if (_stop != null)
            {
                if (InnerCanvas.Children.Contains(_stop))
                {
                    InnerCanvas.Children.Remove(_stop);
                }
            }
            _stop = new Stop
            {
                CancellationTokenSource = source,
                ParentBox = this
            };
            InnerCanvas.Children.Add(_stop);
            _stop.SetValue(Canvas.LeftProperty, 1.0*(Width / 2 - _stop.Width / 2));
            _stop.SetValue(Canvas.TopProperty,20.0);
        }
        /**
         * 这里关联的消息的接受者，LinkId 为接受者Id，LinkType 为接受者类型，
         * **/

        private void MessageInputSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TextArea.Width = ActualWidth - 37 * 2;
            if (_stop != null)
            {
                _stop.SetValue(Canvas.LeftProperty, 1.0*(Width / 2 - _stop.Width / 2));
            }
            
        }

        private void SendMessageInArea(object sender, RoutedEventArgs e)
        {
            if (TextArea.Text.Length > 0)
                SendTextMessage();
        }

        private void SendTextMessage()
        {
            Send?.Invoke(TextArea.Text);
            TextArea.Clear();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //Console.WriteLine("input textchange");
            if(TextArea.Text.Length > 0)
            {
                //sendBt.IsEnabled = true;
                SendBt.Background = new SolidColorBrush(Color.FromRgb(0,0x99,0xff));
                SendBt.BorderBrush = new SolidColorBrush(Color.FromRgb(0,0x99,0xff));
                SendBtBorder.Background = new SolidColorBrush(Color.FromRgb(0,0x99,0xff));
                SendBtBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0,0x99,0xff));
            }
            else
            {
                //sendBt.IsEnabled = false;
                SendBt.Background = new SolidColorBrush(Colors.LightBlue);
                SendBt.BorderBrush = new SolidColorBrush(Colors.LightBlue);
                SendBtBorder.Background = new SolidColorBrush(Colors.LightBlue);
                SendBtBorder.BorderBrush = new SolidColorBrush(Colors.LightBlue);
            }
        }

        private void TextAreaKeyDownPreview(object sender, KeyEventArgs e)
        {
            //Console.WriteLine("input down");
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (TextArea.Text.Length > 0)
                    SendTextMessage();
            }
            // Console.WriteLine("CTRL + ENTER pressed!");
            
        }

        private void ClearContextButtonClick(object sender, RoutedEventArgs e)
        {
            var contextSession = Parent?.SessionContext;
            contextSession?.Clear();
            Parent?.Vm?.ContextPaint();
        }

        private void UseContextSwitchClick(object sender, RoutedEventArgs e)
        {
            if(Parent==null) return;
            if (Parent.UseContext)
            {
                // switch to false
                Parent.UseContext = false;
                ((Image)UseContext.Content).Source = new BitmapImage(new Uri("/images/red.png",UriKind.Relative));
                UseContext.ToolTip = "连续对话已经关闭，点击开启连续对话";
            }
            else
            {
                Parent.UseContext = true;
                ((Image)UseContext.Content).Source = new BitmapImage(new Uri("/images/green.png",UriKind.Relative));
                UseContext.ToolTip = "连续对话已经开启，点击关闭连续对话";
            }
        }
    }
}