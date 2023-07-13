using System.Threading.Tasks;
using System.Windows.Controls;
using Intelligent_AI_Platform.dataCenter;
using Intelligent_AI_Platform.linker;
using Intelligent_AI_Platform.Model.platform.app.GenericChat.chat;
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
        // public new ChatList  Parent
        // {
        //     set;
        //     get;
        // }
        public Session Session
        {
            get => _session;
            set
            {
                _session = value;
                Theme.Content = _session.Theme;
                
                var context = new ContextMenu();
                var item1 = new MenuItem
                {
                    Header = "删除",
                };
                item1.Click += (sender, args) =>
                {
                    var parent = DataCenter.ChatList;
                    DataCenter.SessionGroup.RemoveSession(Session);
                    Task.Run(() =>
                    {
                        SessionGroup.Serialize(DataCenter.SessionGroup,Linker.Location);
                    });
                    parent.UpdateView(true);
                };
                context.Items.Add(item1);
                ContextMenu = context;
            }
        }
    }
}
