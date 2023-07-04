using System.Runtime.Serialization;

namespace OpenAI
{
    [DataContract]
    public enum Participant
    {
        User,
        System,
        Assistant
    }
}