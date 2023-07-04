using System;
using System.Collections.Generic;
using OpenAI.core.based.openaiClient;
using OpenAI.core.builder;
using OpenAI.core.constants;
using OpenAI.instance.chat;
using OpenAI.instance.completion;

namespace OpenAI.instance
{
    /*
     * 当前项目中存在大量的bug，会在以后修护，目前可用的是stream/chat/completion
     * 类似的bug 已经被修复，
     * bug 都是有修护的示例
     * 类型： 错误的使用Dictionary,错误的JsonConvert
     * *
     */
    public class OpenAi: OpenAiClientBase
    {
        // public static string Key;
        // public static string BaseUrl = "https://api.openai-sb.com";
        // public static readonly string V1ChatUrl = "/v1/chat/completions";
        private static readonly OpenAi InnerInstance = new OpenAi();
        private static string _internalApiKey = "";
        public static double InputTokenLimit = 0.6;

        public static string ApiKey
        {
            set
            {
                HeadersBuilder.ApiKey = value;
                _internalApiKey = value;
            }
        }
        public static OpenAi Instance
        {
            get
            {
                if (_internalApiKey == "")
                {
                    throw new Exception("You must set the api key before accessing the instance of this class. Example: OpenAI.apiKey = \"Your API Key\";");
                }

                return InnerInstance;
            }
        }

        public static string Organization
        {
            get =>HeadersBuilder.Organization;
            set => HeadersBuilder.Organization = value;
        }

        public OpenAiChat Chat => new OpenAiChat();
        public OpenAiCompletion OpenAiCompletion => new OpenAiCompletion();

        public static string BaseUrl
        {
            set => OpenAiConfig.BaseUrl = value;
        }
        public static void IncludeHeaders(Dictionary<string, dynamic> headers) {
            HeadersBuilder.IncludeHeaders(headers);
        }
    }
}