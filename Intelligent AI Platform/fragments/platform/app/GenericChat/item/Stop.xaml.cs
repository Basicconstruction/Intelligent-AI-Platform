using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Intelligent_AI_Platform.fragments.platform.app.GenericChat.chatSession;

namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat.item
{
    /// <summary>
    /// Stop.xaml 的交互逻辑
    /// </summary>
    public partial class Stop : UserControl
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
        public Stop()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (_source is { Token: { CanBeCanceled: true } })
            {
                _source.Cancel();
                ParentBox.RemoveStop();
            }
        }
    }
}
