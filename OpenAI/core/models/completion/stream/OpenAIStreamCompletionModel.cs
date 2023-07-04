using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OpenAI.core.models.completion.stream.submodels;

namespace OpenAI.core.models.completion.stream
{
    public class OpenAiStreamCompletionModel
    {
        public readonly string Id;
        public readonly DateTime Created;
        public readonly List<OpenAiStreamCompletionModelChoice> Choices;
        public readonly string Model;
        public bool HaveChoices => Choices.Count != 0;

        public OpenAiStreamCompletionModel(string id, DateTime created, List<OpenAiStreamCompletionModelChoice> choices, string model)
        {
            Id = id;
            Created = created;
            Choices = choices;
            Model = model;
        }

        public static OpenAiStreamCompletionModel FromMap(Dictionary<string, dynamic> json)
        {
            return new OpenAiStreamCompletionModel(
                json["id"],
                DateTime.FromBinary(json["created"]*1000),
                (json["choices"] as List<Dictionary<string,dynamic>>)?.Select(e => OpenAiStreamCompletionModelChoice.FromMap(e))
                .ToList(),
                json["model"]
                );
        }

        public override string ToString()
        {
            return "暂未定义";
        }

        public override bool Equals(object obj)
        {
            if (obj is OpenAiStreamCompletionModel other)
            {
                return Id == other.Id && Created == other.Created &&
                       string.Equals(Model,other.Model) && ListEqual(Choices, other.Choices);
            }

            return false;
        }

        public bool ListEqual(List<OpenAiStreamCompletionModelChoice> one,
            List<OpenAiStreamCompletionModelChoice> theOther)
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
        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Created.GetHashCode() ^ Choices.GetHashCode()
                   ^ Model.GetHashCode();
        }
    }
}