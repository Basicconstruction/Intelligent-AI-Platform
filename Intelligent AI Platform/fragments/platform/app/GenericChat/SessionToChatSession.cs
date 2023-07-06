using System.Collections.Generic;
using Intelligent_AI_Platform.fragments.platform.app.GenericChat.chatSession;
using OpenAI;

namespace Intelligent_AI_Platform.fragments.platform.app.GenericChat
{
    public class SessionToChatSession
    {
        private readonly Dictionary<Session, ChatSession> _map = new Dictionary<Session, ChatSession>();

        public void Put(Session session, ChatSession chatSession)
        {
            _map[session] = chatSession;
        }

        public ChatSession GetChatSession(Session session)
        {
            _map.TryGetValue(session, out var chatSession);
            return chatSession;
        }
    }
}