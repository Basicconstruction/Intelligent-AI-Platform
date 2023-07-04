using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OpenAI.core.models.chat.stream.submodels.choices;

namespace OpenAI.core.models.chat.stream
{
    public class OpenAiStreamChatCompletionModel
    {
        public string Id;
        public DateTime Created;
        public List<OpenAiStreamChatCompletionChoiceModel> Choices;
        private bool HaveChoices => Choices.Count != 0;

        public override int GetHashCode()
        {
            return HashCode;
        }

        private int HashCode =>
            Id.GetHashCode() ^ Created.GetHashCode() ^ Choices.GetHashCode();

        public OpenAiStreamChatCompletionModel(string id, DateTime created, List<OpenAiStreamChatCompletionChoiceModel> choices)
        {
            Id = id;
            Created = created;
            Choices = choices;
        }
        public static OpenAiStreamChatCompletionModel FromMap(Dictionary<string, dynamic> json)
        {
            // var id = json["id"];
            // var date = DateTime.FromBinary(json["created"] * 1000);
            // var list = (JsonConvert.DeserializeObject<List<OpenAiStreamChatCompletionChoiceModel>>(json["choices"]));
            // id += "";
            return new OpenAiStreamChatCompletionModel(
                json["id"],
                DateTime.FromBinary(json["created"]*1000),
                JsonConvert.DeserializeObject<List<OpenAiStreamChatCompletionChoiceModel>>(json["choices"].ToString())//?.Select(e => OpenAiStreamChatCompletionChoiceModel.FromMap(e))
            );
        }

        private bool ListEqual(List<OpenAiStreamChatCompletionChoiceModel> one,
            List<OpenAiStreamChatCompletionChoiceModel> theOther)
        {
            if (one == theOther)
            {
                return true;
            }

            if (one.Count != theOther.Count)
            {
                return false;
            }
            var json1 = JsonConvert.SerializeObject(one);
            var json2 = JsonConvert.SerializeObject(theOther);

            return string.Equals(json1, json2);
        }

        public override string ToString()
        {
            return "暂未定义";
        }

        public override bool Equals(object obj)
        {
            if (obj is OpenAiStreamChatCompletionModel other)
            {
                return Id == other.Id && Created == other.Created && ListEqual(Choices, other.Choices);
            }

            return false;
        }
    }
}