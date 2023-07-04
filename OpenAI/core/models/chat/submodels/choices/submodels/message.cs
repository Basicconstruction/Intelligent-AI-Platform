using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenAI.core.based.entity;

namespace OpenAI.core.models.chat.submodels.choices.submodels
{
    // 忽略了额外的属性
    public class OpenAiChatCompletionChoiceMessageModel
    {
        [JsonProperty("role")]
        public readonly OpenAiChatMessageRole Role;
        [JsonProperty("content")]
        public readonly string Content;
        //public readonly FunctionCallResponse functionCall;
        public OpenAiChatCompletionChoiceMessageModel(OpenAiChatMessageRole role, string content)
        {
            Role = role;
            Content = content;
        }

        public override string ToString()
        {
            return "暂未实现";
        }

        public override bool Equals(object obj)
        {
            if (obj is OpenAiChatCompletionChoiceMessageModel model)
            {
                return Content == model.Content && Role == model.Role;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Role.GetHashCode() ^ Content.GetHashCode();
        }

        public static OpenAiChatCompletionChoiceMessageModel FromMap(Dictionary<string, dynamic> json)
        {
            return new OpenAiChatCompletionChoiceMessageModel(
                role: Parse(json["role"]),
                content:json["content"]??"");
        }

        public Dictionary<string, dynamic> ToMap()
        {
            var dict = new Dictionary<string, dynamic>
            {
                { "role", Role.ToString().ToLower() },
                { "content", Content }
            };
            return dict;
        }
        public static OpenAiChatMessageRole Parse(string str)
        {
            str = str.ToLower();
            switch (str)
            {
                case "system":
                    return OpenAiChatMessageRole.System;
                case "assistant":
                    return OpenAiChatMessageRole.Assistant;
                case "user":
                    return OpenAiChatMessageRole.User;
                default:
                    return OpenAiChatMessageRole.System;
            }
        }
    }
}