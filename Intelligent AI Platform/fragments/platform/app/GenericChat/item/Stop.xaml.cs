using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Intelligent_AI_Platform.fragments.platform.app.GenericChat.chatSession;
using PlatformLib.tools;

namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat.item
{
    /// <summary>
    /// Stop.xaml 的交互逻辑
    /// </summary>
    public partial class Stop
    {
        private CancellationTokenSource _source;

        public CancellationTokenSource CancellationTokenSource
        {
            set => _source = value;
            get => _source;
        }

        public InputBox ParentBox
        {
            set;
            get;
        }

        private readonly RandomProgressBar _bar;
        public Stop()
        {
            InitializeComponent();
            _bar = new RandomProgressBar(200, 30);
            InnerCanvas.Children.Add(_bar);
            Canvas.SetLeft(_bar, 0.0f);
            Canvas.SetTop(_bar, 0.0f);
            _bar.Opacity = 0.6;
            _bar.SetValue(Panel.ZIndexProperty, 0);
            StopBt.SetValue(Panel.ZIndexProperty, 1);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _bar.IsCancelled = true;
            if (_source is { Token: { CanBeCanceled: true } })
            {
                _source.Cancel();
                ParentBox.RemoveStop();
            }
        }

        public void Dispose()
        {
            _bar.IsCancelled = true;
        }
    }
}
