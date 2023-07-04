namespace OpenAI.core.constants
{
    public class OpenAiConfig
    {
        private static string _baseUrl;
        public static string Version => OpenAiStrings.Version;

        public static string BaseUrl
        {
            get=>_baseUrl ?? OpenAiStrings.Version;
            set => _baseUrl = value;
        } 
        
    }
}