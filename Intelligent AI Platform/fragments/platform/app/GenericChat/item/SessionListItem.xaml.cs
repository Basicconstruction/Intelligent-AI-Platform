using System.Windows.Controls;
using OpenAI;

namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat.item
{
    /// <summary>
    /// SessionListItem.xaml 的交互逻辑
    /// </summary>
    public partial class SessionListItem
    {
        public SessionListItem()
        {
            InitializeComponent();
        }

        private Session _session;
        public Session Session
        {
            get => _session;
            set
            {
                _session = value;
                Theme.Content = _session.Theme;
            }
        }
    }
}
