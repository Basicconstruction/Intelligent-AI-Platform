﻿using System.Collections.Generic;
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

        public void Clear()
        {
            _talks.Clear();
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

        public List<Talk> BuildContextFrom(Talk talk)
        {
            var index = _talks.FindIndex(t => t == talk);
            return index == -1 ?
                // The talk was not found in the list
                new List<Talk>() : // Return an empty list
                _talks.GetRange(index, _talks.Count - index);
        }

    }
}