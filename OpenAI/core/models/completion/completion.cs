using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OpenAI.core.models.completion.submodels;

namespace OpenAI.core.models.completion
{
    public class OpenAiCompletionModel
    {
        private readonly string _id;
        private readonly DateTime _created;
        private readonly string _model;
        readonly List<OpenAiCompletionModelChoice> _choices;
        private readonly OpenAiCompletionModelUsage _usage;
        private bool HaveChoices => _choices.Count != 0;

        public override int GetHashCode()
        {
            return HashCode;
        }

        private int HashCode =>
            _id.GetHashCode() ^ _created.GetHashCode() ^ _model.GetHashCode() ^ _choices.GetHashCode();

        public OpenAiCompletionModel(string id, DateTime created, string model, List<OpenAiCompletionModelChoice> choices, OpenAiCompletionModelUsage usage)
        {
            _id = id;
            _created = created;
            _model = model;
            _choices = choices;
            _usage = usage;
        }
        public static OpenAiCompletionModel FromMap(Dictionary<string, dynamic> json)
        {
            return new OpenAiCompletionModel(
                json["id"],
                DateTime.FromBinary(json["created"]*1000),
                json["model"],
                (json["choices"] as List<Dictionary<string,dynamic>>)?.Select(e => OpenAiCompletionModelChoice.FromMap(e))
                .ToList(),
                OpenAiCompletionModelUsage.FromMap(json["usage"])
            );
        }
        public bool ListEqual(List<OpenAiCompletionModelChoice> one,
            List<OpenAiCompletionModelChoice> theOther)
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
            if (obj is OpenAiCompletionModel other)
            {
                return _id == other._id && _created == other._created &&
                       string.Equals(_model,other._model) && ListEqual(_choices, other._choices);
            }

            return false;
        }
    }
}