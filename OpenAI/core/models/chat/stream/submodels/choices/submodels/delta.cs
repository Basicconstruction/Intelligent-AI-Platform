using System.Collections.Generic;
using System.Data;

namespace OpenAI.core.models.chat.stream.submodels.choices.submodels
{
    public class OpenAiStreamChatCompletionChoiceDeltaModel{
        public readonly string Role;
        public readonly string Content;

        public OpenAiStreamChatCompletionChoiceDeltaModel(string role, string content)
        {
            Role = role;
            Content = content;
        }

        public static OpenAiStreamChatCompletionChoiceDeltaModel FromMap(Dictionary<string, dynamic> json)
        {
            return new OpenAiStreamChatCompletionChoiceDeltaModel(
                role: json["role"],
                content: json["content"]
                );
        }

        public Dictionary<string, dynamic> ToMap()
        {
            return new Dictionary<string, dynamic>()
            {
                {"role",Role},
                {"content",Content}
            };
        }

        public override string ToString()
        {
            return "暂未实现";
        }

        public override bool Equals(object obj)
        {
            if (obj is OpenAiStreamChatCompletionChoiceDeltaModel model)
            {
                return model.Content == Content && model.Role == Role;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Role.GetHashCode() ^ Content.GetHashCode();
        }
    }
    
}