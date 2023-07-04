using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OpenAI.core;
using OpenAI.core.builder;
using OpenAI.core.constants;
using OpenAI.core.models.completion;
using OpenAI.core.models.completion.stream;
using OpenAI.core.networking;

namespace OpenAI.instance.completion
{
    public class OpenAiCompletion: IOpenAiCompletionBase
    {
        public string Endpoint => OpenAiStrings.Endpoints.Completion;

        public async Task<OpenAiCompletionModel> Create(string model, [Optional] dynamic prompt, [Optional] string suffix, 
            [Optional] int? maxTokens, [Optional] double? temperature, 
            [Optional] double? topP, [Optional] int? n,
            [Optional] int? logprobs, [Optional] bool? echo, [Optional] string stop,
            [Optional] double? presencePenalty, [Optional] double? frequencyPenalty,
            [Optional] int? bestOf, [Optional] Dictionary<string, dynamic> logitBias,
            [Optional] string user, [Optional] HttpClient client)
        {
            var body = new Dictionary<string, dynamic> { { "model", model } };

            if (prompt != null)
            {
                body.Add("prompt", prompt);
            }

            if (suffix != null)
            {
                body.Add("suffix", suffix);
            }

            if (maxTokens != null)
            {
                body.Add("max_tokens", maxTokens);
            }

            if (temperature != null)
            {
                body.Add("temperature", temperature);
            }

            if (topP != null)
            {
                body.Add("top_p", topP);
            }

            if (n != null)
            {
                body.Add("n", n);
            }

            if (logprobs != null)
            {
                body.Add("logprobs", logprobs);
            }

            if (echo != null)
            {
                body.Add("echo", echo);
            }

            if (stop != null)
            {
                body.Add("stop", stop);
            }

            if (presencePenalty != null)
            {
                body.Add("presence_penalty", presencePenalty);
            }

            if (frequencyPenalty != null)
            {
                body.Add("frequency_penalty", frequencyPenalty);
            }

            if (bestOf != null)
            {
                body.Add("best_of", bestOf);
            }

            if (logitBias != null)
            {
                body.Add("logit_bias", logitBias);
            }

            if (user != null)
            {
                body.Add("user", user);
            }

            return await OpenAiNetworkingClient.Post<OpenAiCompletionModel>(
                BaseApiUrlBuilder.Build(Endpoint),
                (response) => OpenAiCompletionModel.FromMap(response)
                ,body
                );
        }

        public async Task<IObservable<OpenAiStreamCompletionModel>> CreateStream(string model, [Optional] dynamic prompt, [Optional] string suffix, 
            [Optional] int? maxTokens, [Optional] double? temperature, 
            [Optional] double? topP, [Optional] int? n,
            [Optional] int? logprobs, [Optional] bool? echo, [Optional] string stop,
            [Optional] double? presencePenalty, [Optional] double? frequencyPenalty,
            [Optional] int? bestOf, [Optional] Dictionary<string, dynamic> logitBias,
            [Optional] string user, [Optional] HttpClient client)
        {
            var body = new Dictionary<string, dynamic>
            {
                { "model", model },
                { "stream", true }
            };
            if (prompt != null)
            {
                body.Add("prompt", prompt);
            }

            if (suffix != null)
            {
                body.Add("suffix", suffix);
            }

            if (maxTokens != null)
            {
                body.Add("max_tokens", maxTokens);
            }

            if (temperature != null)
            {
                body.Add("temperature", temperature);
            }

            if (topP != null)
            {
                body.Add("top_p", topP);
            }

            if (n != null)
            {
                body.Add("n", n);
            }

            if (logprobs != null)
            {
                body.Add("logprobs", logprobs);
            }

            if (echo != null)
            {
                body.Add("echo", echo);
            }

            if (stop != null)
            {
                body.Add("stop", stop);
            }

            if (presencePenalty != null)
            {
                body.Add("presence_penalty", presencePenalty);
            }

            if (frequencyPenalty != null)
            {
                body.Add("frequency_penalty", frequencyPenalty);
            }

            if (bestOf != null)
            {
                body.Add("best_of", bestOf);
            }

            if (logitBias != null)
            {
                body.Add("logit_bias", logitBias);
            }

            if (user != null)
            {
                body.Add("user", user);
            }

            return await OpenAiNetworkingClient.PostStream<OpenAiStreamCompletionModel>(
                BaseApiUrlBuilder.Build(Endpoint),
                body,
                response=>OpenAiStreamCompletionModel.FromMap(response)
                );
        }

        public async Task<IObservable<string>> CreateStreamText(string model, [Optional] dynamic prompt, [Optional] string suffix, 
            [Optional] int? maxTokens, [Optional] double? temperature, 
            [Optional] double? topP, [Optional] int? n,
            [Optional] int? logprobs, [Optional] bool? echo, [Optional] string stop,
            [Optional] double? presencePenalty, [Optional] double? frequencyPenalty,
            [Optional] int? bestOf, [Optional] Dictionary<string, dynamic> logitBias,
            [Optional] string user, [Optional] HttpClient client)
        {
            IObservable<OpenAiStreamCompletionModel> stream = await CreateStream(
                model: model,
                prompt: prompt,
                suffix: suffix,
                maxTokens: maxTokens,
                temperature: temperature,
                topP: topP,
                n: n,
                logprobs: logprobs,
                echo: echo,
                stop: stop,
                presencePenalty: presencePenalty,
                frequencyPenalty: frequencyPenalty,
                bestOf: bestOf,
                logitBias: logitBias,
                user: user);
            return stream.SelectMany(m=>Observable.Return(m.Choices.First().Text));
        }
    }
}