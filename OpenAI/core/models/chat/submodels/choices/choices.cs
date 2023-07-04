using System.Collections.Generic;
using Newtonsoft.Json;
using OpenAI.core.models.chat.submodels.choices.submodels;

namespace OpenAI.core.models.chat.submodels.choices
{
    public class OpenAiChatCompletionChoiceModel
    {
        [JsonProperty("index")]
        public readonly int Index;
        [JsonProperty("delta")]
        public readonly OpenAiChatCompletionChoiceMessageModel Message;
        [JsonProperty("finish_reason")]
        public readonly string FinishReason;

        public OpenAiChatCompletionChoiceModel(int index, OpenAiChatCompletionChoiceMessageModel message, string finishReason)
        {
            Index = index;
            Message = message;
            FinishReason = finishReason;
        }

        public static OpenAiChatCompletionChoiceModel FromMap(Dictionary<string, dynamic> json)
        {
            return new OpenAiChatCompletionChoiceModel(
                index: json["index"],
                message: OpenAiChatCompletionChoiceMessageModel.FromMap(json["message"]),
                finishReason: json["finish_reason"]
            );
        }

        public Dictionary<string, dynamic> ToMap()
        {
            return new Dictionary<string, dynamic>()
            {
                {"index", Index},
                {"message",Message.ToMap()},
                {"finish_reason",FinishReason}
            };
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}