using System;
using System.Windows.Controls;
using Intelligent_AI_Platform.dataCenter;
using Intelligent_AI_Platform.fragments.platform.app.GenericChat.chatSession;
using Intelligent_AI_Platform.fragments.platform.app.GenericChat.item;
using Intelligent_AI_Platform.linker;
using Intelligent_AI_Platform.linker.manager;
using Intelligent_AI_Platform.Model.platform.app.GenericChat.chat;
using Intelligent_AI_Platform.pages.platform.app.GenericChat.dialog.setting;
using OpenAI;
using PlatformLib.ui.framework;
using PlatformLib.ui.framework.enumList;
using PlatformLib.ui.framework.fragment;

namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat
{
    public partial class ChatList: IFragment,ISizeBox
    {
        public ChatList()
        {
            InitializeComponent();
            Init();
        }

        public SessionGroup SessionGroup
        {
            get;
            set;
        }
        public void Init()
        {
            SessionGroup = DataCenter.SessionGroup;
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public void UpdateView()
        {
            
        }

        // ReSharper disable once MethodOverloadWithOptionalParameter
        public new void UpdateView(bool change = false)
        {
            ChatTopicList.Items.Clear();
            foreach (var session in SessionGroup.Group)
            {
                ChatTopicList.Items.Add(new SessionListItem() {Session = session});
            }

            if (change)
            {
                ChatTopicList.SelectedIndex = 0;
            }
        }

        public double PreferredWidth
        {
            get => 200;
            set => throw new NotImplementedException();
        }
        public double PreferredHeight
        {
            get => 720;
            set => throw new NotImplementedException();
        }
        public double SWidth { get => Width; set => Width = value; }
        public double SHeight { get => Height; set => Height = value; }
        public double SMinWidth { get => MinWidth; set => MinWidth = value; }
        public double SMaxWidth { get => MaxWidth; set => MaxHeight = value; }
        public double SMinHeight { get => MinHeight; set => MinHeight = value; }
        public double SMaxHeight { get => MaxHeight; set => MaxHeight = value; }
        public double Flexibility { get => 1.0; set => throw new NotImplementedException(); }

        private void ChatListSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            ChatTopicList.Height = ActualHeight - 117;
        }


        private void NewChat_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SessionGroup.InsertNewSession();
            UpdateView();
            //Linker.NavigatorManager.GetActivityManager(linker.manager.NavigatorLabel.Root).Replace(new ChatActivity(), PlatformLib.ui.framework.enumList.PutStyle.FitParent);
        }

        private void SelectNewSession(object sender, SelectionChangedEventArgs e)
        {
            var listView = (ListView)sender;
            if (listView == null) return;
            var select = listView.SelectedIndex;
            if(select<0||select>SessionGroup.Group.Count-1) return;
            var session = SessionGroup.Group[select];
            var chatSession = DataCenter.SessionToChatSession.GetChatSession(session);
            if (chatSession != null)
            {
                var context = DataCenter.SessionToContext.GetContext(session);
                if (context != null)
                {
                }
                else
                {
                    context = new SessionContext();
                    DataCenter.SessionToContext.Put(session,context); 
                }
                Linker.NavigatorManager.GetActivityManager(NavigatorLabel.Chat).Replace(chatSession,PutStyle.FitParent);
            }
            else
            {
                var context = DataCenter.SessionToContext.GetContext(session);
                if (context != null)
                {
                }
                else
                {
                    context = new SessionContext();
                    DataCenter.SessionToContext.Put(session,context); 
                }

                chatSession = new ChatSession(){Session = session,SessionContext=context};
                DataCenter.SessionToChatSession.Put(session,chatSession);
                Linker.NavigatorManager.GetActivityManager(NavigatorLabel.Chat).Replace(chatSession,PutStyle.FitParent);
            }
            //var chatSession = new ChatSession() { Session = session, SessionContext = new SessionContext() };
            //Linker.NavigatorManager.GetActivityManager(NavigatorLabel.Chat).Replace(,PutStyle.FitParent);
        }

        private void Settings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new Setting();
            dialog.ShowDialog();
        }
    }
}