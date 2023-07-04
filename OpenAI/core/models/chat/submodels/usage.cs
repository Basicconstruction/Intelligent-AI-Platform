using System.Collections.Generic;

namespace OpenAI.core.models.chat.submodels
{
    public class OpenAiChatCompletionUsageModel
    {
        public readonly int PromptTokens;
        public readonly int CompletionTokens;
        public readonly int TotalTokens;

        public OpenAiChatCompletionUsageModel(int promptTokens, int completionTokens, int totalTokens)
        {
            PromptTokens = promptTokens;
            CompletionTokens = completionTokens;
            TotalTokens = totalTokens;
        }

        public override string ToString()
        {
            return
                "OpenAICompletionModelUsage(promptTokens: "+PromptTokens+", completionTokens: "+CompletionTokens+", totalTokens: "+TotalTokens+")";
        }

        public override bool Equals(object obj)
        {
            if (obj is OpenAiChatCompletionUsageModel other)
            {
                return PromptTokens == other.PromptTokens
                       && CompletionTokens == other.CompletionTokens
                       && TotalTokens == other.TotalTokens;
            }

            return false;
        }

        public Dictionary<string, dynamic> ToMap()
        {
            return new Dictionary<string, dynamic>()
            {
                {"prompt_tokens",PromptTokens},
                {"completion_tokens",CompletionTokens},
                {"total_tokens",TotalTokens},
            };
        }

        public static OpenAiChatCompletionUsageModel FromMap(Dictionary<string, dynamic> json)
        {
            return new OpenAiChatCompletionUsageModel(
                json["prompt_tokens"],
                json["completion_tokens"],
                json["total_tokens"]
            );
        }
        public override int GetHashCode()
        {
            return PromptTokens.GetHashCode() ^ CompletionTokens.GetHashCode() ^ TotalTokens.GetHashCode();
        }
    }
}