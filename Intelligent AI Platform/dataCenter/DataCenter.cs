using System.Windows;
using Intelligent_AI_Platform.config;
using Intelligent_AI_Platform.fragments.platform.app.GenericChat;
using Intelligent_AI_Platform.fragments.platform.app.GenericChat.chatSession;
using Intelligent_AI_Platform.Model.platform.app.GenericChat.chat;
using OpenAI;

namespace Intelligent_AI_Platform.dataCenter
{
    public class DataCenter
    {
        private const string SessionGroupKey = "sessionGroup";
        private const string ConfigurationKey = "configuration";
        private const string ChatListKey = "chatList";
        private const string ChatSessionKey = "chatSession";
        private const string SessionToChatKey = "sessionMap";
        private const string SessionToContextKey = "sessionToContext";

        public static SessionGroup SessionGroup
        {
            get => (SessionGroup)Application.Current.Properties[SessionGroupKey];
            set => Application.Current.Properties[SessionGroupKey] = value;
        }

        public static Configuration Configuration
        {
            get => (Configuration)Application.Current.Properties[ConfigurationKey];
            set => Application.Current.Properties[ConfigurationKey] = value;
        }

        public static ChatList ChatList
        {
            get => (ChatList)Application.Current.Properties[ChatListKey];
            set => Application.Current.Properties[ChatListKey] = value;
        }

        public static ChatSession ChatSession
        {
            get => (ChatSession)Application.Current.Properties[ChatSessionKey];
            set => Application.Current.Properties[ChatSessionKey] = value;
        }

        public static SessionToChatSession SessionToChatSession
        {
            get => (SessionToChatSession)Application.Current.Properties[SessionToChatKey];
            set => Application.Current.Properties[SessionToChatKey] = value;
        }

        public static SessionToContext SessionToContext
        {
            get => (SessionToContext)Application.Current.Properties[SessionToContextKey];
            set => Application.Current.Properties[SessionToContextKey] = value;
        }
        
    }
}