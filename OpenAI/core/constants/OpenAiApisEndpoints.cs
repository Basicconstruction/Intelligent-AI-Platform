namespace OpenAI.core.constants
{
    public class OpenAiApisEndpoints
    {
        public readonly string Completion = "/completions";
        public readonly string Audio = "/audio";
        public readonly string Chat = "/chat/completions";
        public readonly string Edits = "/edits";
        public readonly string Embeddings = "/embeddings";
        public readonly string Files = "/files";
        public readonly string FineTunes = "/fine-tunes";
        public readonly string Images = "/images";
        public readonly string Models = "/models";
        public readonly string Moderation = "/moderations";

        private static OpenAiApisEndpoints _instance = new OpenAiApisEndpoints();
        public static OpenAiApisEndpoints Instance => _instance;
    }
}