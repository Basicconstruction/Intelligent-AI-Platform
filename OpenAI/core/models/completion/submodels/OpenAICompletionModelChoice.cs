using System.Collections.Generic;

namespace OpenAI.core.models.completion.submodels
{
    public class OpenAiCompletionModelChoice
    {
        private readonly string _text;

        /// The index of the choice.
        private readonly int _index;

        /// The log probabilities of the tokens in the completion.
        private readonly int _logprobs;

        /// The reason the completion finished.
        private readonly string _finishReason;

        public OpenAiCompletionModelChoice(string text, int index, int logprobs, string finishReason)
        {
            _text = text;
            _index = index;
            _logprobs = logprobs;
            _finishReason = finishReason;
        }

        public static OpenAiCompletionModelChoice FromMap(Dictionary<string, dynamic> json)
        {
            return new OpenAiCompletionModelChoice(
                json["text"],
                json["index"],
                json["logprobs"],
                json["finishReason"]
            );
        }

        public override string ToString()
        {
            return "OpenAICompletionModelChoice(text: "+_text+", index: "+_index+", logprobs: "+_logprobs+", finishReason: "+_finishReason+")";
        }

        public override bool Equals(object obj)
        {
            if (obj is OpenAiCompletionModelChoice other)
            {
                return other._text == _text &&
                       other._index == _index &&
                       other._logprobs == _logprobs &&
                       other._finishReason == _finishReason;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _text.GetHashCode() ^
                   _index.GetHashCode() ^
                   _logprobs.GetHashCode() ^
                   _finishReason.GetHashCode();
        }
    }
}