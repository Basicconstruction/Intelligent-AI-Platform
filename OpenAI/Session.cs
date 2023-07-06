using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace OpenAI
{
    [DataContract]
    public class Session
    {
        [DataMember]
        public string Theme
        {
            set;
            get;
        }

        public static readonly string DefaultTheme = "新的聊天";

        public static string GetTheme(Session session)
        {
            var talk = session.Talks.FirstOrDefault(t => t.Participant == Participant.User);
            return talk?.Content;
        }

        [DataMember]
        private readonly List<Talk> _talks = new List<Talk>();

        public Talk GeTalkByHashCode(long time)
        {
            return _talks.FirstOrDefault(t => t.Time == time);
        }

        public List<Talk> Talks => _talks;

        public void InsertTalk(Talk talk)
        {
            if (_talks.FirstOrDefault(t => t.Time == talk.Time) != null) return;
            if (Theme == "")
            {
                Theme = talk.Content;
            }
            _talks.Add(talk);
        }
        
    }
}