namespace OpenAI.core.based.entity
{
    public enum OpenAiImageSize { Size256, Size512, Size1024 }

    public enum OpenAiImageResponseFormat { Url, B64Json }

    public enum OpenAiAudioResponseFormat { Json, Text, Srt, VerboseJson, Vtt }

    public enum OpenAiChatMessageRole { System, User, Assistant, Function }
}