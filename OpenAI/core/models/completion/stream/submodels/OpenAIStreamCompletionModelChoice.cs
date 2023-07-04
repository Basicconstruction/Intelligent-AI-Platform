using System.Collections.Generic;

namespace OpenAI.core.models.completion.stream.submodels
{
    public class OpenAiStreamCompletionModelChoice
    {
        public readonly string Text;

        /// The index of the choice.
        public readonly int Index;

        /// The log probabilities of the tokens in the completion.
        public readonly int Logprobs;

        /// The reason the completion finished.
        public readonly string FinishReason;

        public OpenAiStreamCompletionModelChoice(string text, int index, int logprobs, string finishReason)
        {
            Text = text;
            Index = index;
            Logprobs = logprobs;
            FinishReason = finishReason;
        }

        public static OpenAiStreamCompletionModelChoice FromMap(Dictionary<string, dynamic> json)
        {
            return new OpenAiStreamCompletionModelChoice(
                json["text"],
                json["index"],
                json["logprobs"],
                json["finishReason"]
            );
        }

        public override string ToString()
        {
            return "OpenAIStreamCompletionModelChoice(text: "+Text+", index: "+Index+", logprobs: "+Logprobs+", finishReason: "+FinishReason+")";
        }

        public override bool Equals(object obj)
        {
            if (obj is OpenAiStreamCompletionModelChoice other)
            {
                return other.Text == Text &&
                       other.Index == Index &&
                       other.Logprobs == Logprobs &&
                       other.FinishReason == FinishReason;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode() ^
                   Index.GetHashCode() ^
                   Logprobs.GetHashCode() ^
                   FinishReason.GetHashCode();
        }
    }
}