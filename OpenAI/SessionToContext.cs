using System.Collections.Generic;

namespace OpenAI
{
    public class SessionToContext
    {
        private readonly Dictionary<Session, SessionContext> _map = new Dictionary<Session, SessionContext>();
        public void Put(Session session, SessionContext context)
        {
            _map[session] = context;
        }

        public SessionContext GetContext(Session session)
        {
            return _map.TryGetValue(session, out var context) ? context : null;
        }
    }
}