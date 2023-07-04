using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using OpenAI.core.based.chat;
using OpenAI.core.builder;
using OpenAI.core.constants;
using OpenAI.core.models.chat;
using OpenAI.core.models.chat.stream;
using OpenAI.core.models.chat.submodels.choices.submodels;
using OpenAI.core.networking;

namespace OpenAI.instance.chat
{
    public class OpenAiChat: IOpenAiChatBase
    {
        public string Endpoint => OpenAiStrings.Endpoints.Chat;

        public async Task<OpenAiChatCompletionModel> Create(string model, List<OpenAiChatCompletionChoiceMessageModel> messages, double temperature = 0, double topP = 0, int n = 0, dynamic stop = null,
            int maxTokens = 0, double presencePenalty = 0, double frequencyPenalty = 0, Dictionary<string, dynamic> logitBias = null,
            string user = null, HttpClient client = null)
        {
            var body = new Dictionary<string, dynamic>
            {
                { "model", model },
            };
            var msg = messages.Select(m => m.ToMap()).ToList();
            if (msg.Count != 0)
            {
                body.Add("messages",msg);
            }
            if (maxTokens != 0)
            {
                body.Add("max_tokens", maxTokens);
            }

            if (temperature != 0)
            {
                body.Add("temperature", temperature);
            }

            if (topP != 0)
            {
                body.Add("top_p", topP);
            }

            if (n != 0)
            {
                body.Add("n", n);
            }

            if (stop != null)
            {
                body.Add("stop", stop);
            }

            if (presencePenalty != 0)
            {
                body.Add("presence_penalty", presencePenalty);
            }

            if (frequencyPenalty != 0)
            {
                body.Add("frequency_penalty", frequencyPenalty);
            }

            if (logitBias != null)
            {
                body.Add("logit_bias", logitBias);
            }

            if (user != null)
            {
                body.Add("user", user);
            }
            return await OpenAiNetworkingClient.Post<OpenAiChatCompletionModel>(
                to: BaseApiUrlBuilder.Build(Endpoint),
                body: body,
                onSuccess: response => OpenAiChatCompletionModel.FromMap(response),
                client: client
                
            );
        }

        public async Task<Stream> CreateStream2(string model, List<OpenAiChatCompletionChoiceMessageModel> messages,
            double temperature = 0, double topP = 0, int n = 0, dynamic stop = null,
            int maxTokens = 0, double presencePenalty = 0, double frequencyPenalty = 0,
            Dictionary<string, dynamic> logitBias = null,
            string user = null)
        {
            var body = new Dictionary<string, dynamic>
            {
                { "model", model },
                {"stream",true}
            };
            var msg = messages.Select(m => m.ToMap()).ToList();
            if (msg.Count != 0)
            {
                body.Add("messages",msg);
            }
            if (maxTokens != 0)
            {
                body.Add("max_tokens", maxTokens);
            }

            if (temperature != 0)
            {
                body.Add("temperature", temperature);
            }

            if (topP != 0)
            {
                body.Add("top_p", topP);
            }

            if (n != 0)
            {
                body.Add("n", n);
            }

            if (stop != null)
            {
                body.Add("stop", stop);
            }

            if (presencePenalty != 0)
            {
                body.Add("presence_penalty", presencePenalty);
            }

            if (frequencyPenalty != 0)
            {
                body.Add("frequency_penalty", frequencyPenalty);
            }

            if (logitBias != null)
            {
                body.Add("logit_bias", logitBias);
            }

            if (user != null)
            {
                body.Add("user", user);
            }

            return await OpenAiNetworkingClient.NewStream(
                to: BaseApiUrlBuilder.Build(Endpoint),
                body: body
                );
        }
        public async Task<IObservable<OpenAiStreamChatCompletionModel>> CreateStream(string model, List<OpenAiChatCompletionChoiceMessageModel> messages, double temperature = 0, double topP = 0, int n = 0, dynamic stop = null,
            int maxTokens = 0, double presencePenalty = 0, double frequencyPenalty = 0, Dictionary<string, dynamic> logitBias = null,
            string user = null, HttpClient client = null)
        {
            var body = new Dictionary<string, dynamic>
            {
                { "model", model },
                {"stream",true}
            };
            var msg = messages.Select(m => m.ToMap()).ToList();
            if (msg.Count != 0)
            {
                body.Add("messages",msg);
            }
            if (maxTokens != 0)
            {
                body.Add("max_tokens", maxTokens);
            }

            if (temperature != 0)
            {
                body.Add("temperature", temperature);
            }

            if (topP != 0)
            {
                body.Add("top_p", topP);
            }

            if (n != 0)
            {
                body.Add("n", n);
            }

            if (stop != null)
            {
                body.Add("stop", stop);
            }

            if (presencePenalty != 0)
            {
                body.Add("presence_penalty", presencePenalty);
            }

            if (frequencyPenalty != 0)
            {
                body.Add("frequency_penalty", frequencyPenalty);
            }

            if (logitBias != null)
            {
                body.Add("logit_bias", logitBias);
            }

            if (user != null)
            {
                body.Add("user", user);
            }
            return await OpenAiNetworkingClient.PostStream<OpenAiStreamChatCompletionModel>(
                to: BaseApiUrlBuilder.Build(Endpoint),
                body: body,
                onSuccess: response => OpenAiStreamChatCompletionModel.FromMap(response),
                client: client
            );
        }
    }
}