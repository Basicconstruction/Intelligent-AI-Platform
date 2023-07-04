using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OpenAI.core.models.chat.submodels;
using OpenAI.core.models.chat.submodels.choices;

namespace OpenAI.core.models.chat
{
    public class OpenAiChatCompletionModel
    {
        [JsonProperty("id")]
        public readonly string Id;
        [JsonProperty("created")]
        public readonly DateTime Created;
        [JsonProperty("choices")]
        public readonly List<OpenAiChatCompletionChoiceModel> Choices;
        [JsonProperty("usage")]
        public readonly OpenAiChatCompletionUsageModel Usage;
        private bool HaveChoices => Choices.Count == 0;

        public OpenAiChatCompletionModel(string id, DateTime created, List<OpenAiChatCompletionChoiceModel> choices, OpenAiChatCompletionUsageModel usage)
        {
            Id = id;
            Created = created;
            Choices = choices;
            Usage = usage;
        }

        public static OpenAiChatCompletionModel FromMap(Dictionary<string, dynamic> json)
        {
            return new OpenAiChatCompletionModel(
                id:json["id"],
                created: DateTime.FromBinary(json["created"]*1000),
                choices: JsonConvert.DeserializeObject<List<OpenAiChatCompletionChoiceModel>>(json["choices"].ToString()),
                // ((json["choices"] as List<Dictionary<string,dynamic>>) ?? throw new InvalidOperationException()).
                // Select(e
                //     => OpenAiChatCompletionChoiceModel.FromMap(e)).ToList(),
                usage: OpenAiChatCompletionUsageModel.FromMap(json["usage"])
            );
        }

        public Dictionary<string, dynamic> ToMap()
        {
            return new Dictionary<string, dynamic>()
            {
                {"id",Id},
                {"created",Created.ToBinary()},
                {"choices",Choices.Select(e=>e.ToMap()).ToList()},
                {"usage",Usage.ToMap()}
            };
        }
        public override string ToString()
        {
            return "暂未实现";
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Created.GetHashCode() ^ Choices.GetHashCode() ^ Usage.GetHashCode();
        }
        
    }
}