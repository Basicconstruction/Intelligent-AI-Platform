namespace OpenAI.core.constants
{
    public class OpenAiStrings
    {
        public const string Openai = "OpenAI";
        public const string Version = "v1";
        public const string DefaultBaseUrl = "https://api.openai.com";
        public const string GetMethod = "GET";
        public const string PostMethod = "POST";
        public const string StreamResponseStart = "data: ";
        public const string StreamResponseEnd = "[DONE]";
        public const string ErrorFieldKey = "error";
        public const string MessageFieldKey = "message";
        public static readonly OpenAiApisEndpoints Endpoints = OpenAiApisEndpoints.Instance;
    } 
}