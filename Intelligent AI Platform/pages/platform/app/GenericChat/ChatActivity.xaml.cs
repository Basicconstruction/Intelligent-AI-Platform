using System;
using Intelligent_AI_Platform.config;
using Intelligent_AI_Platform.dataCenter;
using Intelligent_AI_Platform.fragments.platform.app.GenericChat;
using Intelligent_AI_Platform.fragments.platform.app.GenericChat.chatSession;
using Intelligent_AI_Platform.fragments.platform.app.GenericChat.item;
using Intelligent_AI_Platform.linker;
using Intelligent_AI_Platform.linker.manager;
using Intelligent_AI_Platform.Model.platform.app.GenericChat.chat;
using OpenAI;
using OpenAI.instance;
using PlatformLib.ui.framework;
using PlatformLib.ui.framework.enumList;

namespace Intelligent_AI_Platform.pages.platform.app.GenericChat
{
    /// <summary>
    /// Chat.xaml 的交互逻辑
    /// </summary>
    public partial class ChatActivity : IActivity, ISizeBox
    {
        public ChatActivity()
        {
            InitializeComponent();
            Init();
        }
        public double PreferredWidth
        {
            get => 960;
            set => throw new NotImplementedException();
        }
        public double PreferredHeight
        {
            get => 610;
            set => throw new NotImplementedException();
        }
        public double SWidth { get => Width; set => Width = value; }
        public double SHeight { get => Height; set => Height = value; }
        public double SMinWidth { get => MinWidth; set => MinWidth = value; }
        public double SMaxWidth { get => MaxWidth; set => MaxHeight = value; }
        public double SMinHeight { get => MinHeight; set => MinHeight = value; }
        public double SMaxHeight { get => MaxHeight; set => MaxHeight = value; }
        public double Flexibility { get => 1.0; set => throw new NotImplementedException(); }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            // 初始化字典
            DataCenter.SessionToContext = new SessionToContext();
            DataCenter.SessionToChatSession = new SessionToChatSession();
            // 载入和初始化设置和对话列表
            var config = Configuration.LoadConfiguration(Linker.Location);
            if (config == null)
            {
                DataCenter.Configuration = new Configuration();
                DataCenter.Configuration.TurnsTo(new Configuration());
                Configuration.Serialize(DataCenter.Configuration,Linker.Location);
                if (DataCenter.Configuration.ProviderUrl != "")
                {
                    OpenAi.BaseUrl = DataCenter.Configuration.ProviderUrl;
                }

                if (DataCenter.Configuration.Key != "")
                {
                    OpenAi.ApiKey = DataCenter.Configuration.Key;
                }
            }
            else
            {
                DataCenter.Configuration = config;
                if (DataCenter.Configuration.ProviderUrl != "")
                {
                    OpenAi.BaseUrl = DataCenter.Configuration.ProviderUrl;
                }

                if (DataCenter.Configuration.Key != "")
                {
                    OpenAi.ApiKey = DataCenter.Configuration.Key;
                }
            }
            var sessionGroup = SessionGroup.LoadSessionGroup(Linker.Location);
            DataCenter.SessionGroup = sessionGroup ?? new SessionGroup();

            if (DataCenter.SessionGroup.Group.Count < 1)
            {
                DataCenter.SessionGroup.Group.Add(new Session(){Theme = Session.DefaultTheme});
            }
            // 初始化菜单
            ChatMenu.Push(new ChatMenu(),PutStyle.FitParent);
            // 初始化消息列表
            var chatList = new ChatList();
            DataCenter.ChatList = chatList;
            foreach (var session in DataCenter.SessionGroup.Group)
            {
                chatList.ChatTopicList.Items.Add(new SessionListItem() {Session = session});
            }
            ChatList.Push(chatList,PutStyle.FitParent);

            //初始化 会话
            var session1 = DataCenter.SessionGroup.Group[0];
            var context1 = new SessionContext();
            DataCenter.SessionToContext.Put(session1, context1);
            var chatSession = new ChatSession()
            {
                Session = session1, SessionContext = context1
            };
            ChatSession.Push(chatSession,PutStyle.FitParent);
            DataCenter.SessionToChatSession.Put(session1,chatSession);
            // 注册 聊天会话容器
            var navigator = Linker.NavigatorManager;
            navigator.Register(NavigatorLabel.Chat, ChatSession);
        }

        public void UpdateView()
        {
            throw new NotImplementedException();
        }
        private void ChatActivitySizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            ChatMenu.SWidth = ActualWidth;
            ChatList.SHeight = ActualHeight - ChatMenu.Height;
            ChatSession.SHeight = ActualHeight - ChatMenu.Height;
            ChatSession.SWidth = ActualWidth- ChatList.Width; 
        }
    }
}
