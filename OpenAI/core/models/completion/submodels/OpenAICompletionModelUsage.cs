using System.Collections.Generic;

namespace OpenAI.core.models.completion.submodels
{
    public class OpenAiCompletionModelUsage
    {
        public readonly int PromptTokens;
        public readonly int CompletionTokens;
        public readonly int TotalTokens;

        public OpenAiCompletionModelUsage(int promptTokens, int completionTokens, int totalTokens)
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

        public Dictionary<string, dynamic> ToMap()
        {
            return new Dictionary<string, dynamic>()
            {
                {"prompt_tokens",PromptTokens},
                {"completion_tokens",CompletionTokens},
                {"total_tokens",TotalTokens},
            };
        }
        public override bool Equals(object obj)
        {
            if (obj is OpenAiCompletionModelUsage other)
            {
                return PromptTokens == other.PromptTokens
                       && CompletionTokens == other.CompletionTokens
                       && TotalTokens == other.TotalTokens;
            }

            return false;
        }

        public static OpenAiCompletionModelUsage FromMap(Dictionary<string, dynamic> json)
        {
            return new OpenAiCompletionModelUsage(
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