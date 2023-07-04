using System;
using System.Windows.Forms;
using Intelligent_AI_Platform.config;
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
            var config = Configuration.LoadConfiguration(Linker.Location);
            if (config == null)
            {
                Linker.Configuration = new Configuration();
                Linker.Configuration.TurnsTo(new Configuration());
                Configuration.Serialize(Linker.Configuration,Linker.Location);
                if (Linker.Configuration.ProviderUrl != "")
                {
                    OpenAi.BaseUrl = Linker.Configuration.ProviderUrl;
                }

                if (Linker.Configuration.Key != "")
                {
                    OpenAi.ApiKey = Linker.Configuration.Key;
                }
            }
            else
            {
                Linker.Configuration = config;
                if (Linker.Configuration.ProviderUrl != "")
                {
                    OpenAi.BaseUrl = Linker.Configuration.ProviderUrl;
                }

                if (Linker.Configuration.Key != "")
                {
                    OpenAi.ApiKey = Linker.Configuration.Key;
                }
            }

            var sessionGroup = SessionGroup.LoadSessionGroup(Linker.Location);
            if (sessionGroup == null)
            {
                Linker.SessionGroup = new SessionGroup();
            }
            else
            {
                Linker.SessionGroup = sessionGroup;
            }

            if (Linker.SessionGroup.Group.Count < 1)
            {
                Linker.SessionGroup.Group.Add(new Session(){Theme = "我的聊天"});
            }
            ChatMenu.Push(new ChatMenu(),PutStyle.FitParent);
            var chatList = new ChatList();
            foreach (var session in Linker.SessionGroup.Group)
            {
                chatList.ChatTopicList.Items.Add(new SessionListItem() {Session = session});
            }
            ChatList.Push(chatList,PutStyle.FitParent);
            ChatSession.Push(new ChatSession()
            {
                Session = Linker.SessionGroup.Group[0],SessionContext = new OpenAI.SessionContext()
            },PutStyle.FitParent);
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
