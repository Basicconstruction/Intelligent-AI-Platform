using System.Collections.Generic;
using OpenAI.core.models.chat.stream.submodels.choices.submodels;

namespace OpenAI.core.models.chat.stream.submodels.choices
{
    public class OpenAiStreamChatCompletionChoiceModel
    {
        public int index;
        public OpenAiStreamChatCompletionChoiceDeltaModel delta;
        public string finishReason;

        public OpenAiStreamChatCompletionChoiceModel(int index, OpenAiStreamChatCompletionChoiceDeltaModel delta, string finishReason)
        {
            this.index = index;
            this.delta = delta;
            this.finishReason = finishReason;
        }

        public static OpenAiStreamChatCompletionChoiceModel FromMap(Dictionary<string,dynamic> json)
        {
            return new OpenAiStreamChatCompletionChoiceModel(
                index: json["index"],
                delta: OpenAiStreamChatCompletionChoiceDeltaModel.FromMap(json["delta"]),
                finishReason: json["finish_reason"]
                );
        }
        public override int GetHashCode()
        {
            return index.GetHashCode() ^ delta.GetHashCode() ^ finishReason.GetHashCode();
        }

        public override string ToString()
        {
            return "暂未实现";
        }

        public override bool Equals(object obj)
        {
            if (obj is OpenAiStreamChatCompletionChoiceModel model)
            {
                return model.delta == delta && model.index == index && model.finishReason == finishReason;
            }

            return false;
        }
    }
}