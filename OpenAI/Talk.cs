using System;
using System.Runtime.Serialization;

namespace OpenAI
{
    [DataContract]
    public class Talk
    {
        [DataMember]
        public long Time;
        [DataMember]
        public readonly Participant Participant;
        [DataMember]
        public string Name;
        [DataMember]
        public string Content;

        public Talk()
        {
        }

        public Talk(Participant participant, string name, string content)
        {
            Participant = participant;
            Name = name;
            Content = content;
            Time = DateTime.Now.Millisecond;
        }

        public Talk(Participant participant, string content)
        {
            Participant = participant;
            switch (Participant)
            {
                case Participant.User:
                    Name = "user";
                    break;
                case Participant.Assistant:
                    Name = "assistant";
                    break;
                case Participant.System:
                default:
                    Name = "system";
                    break;
            }

            Content = content;
        }
    }
}