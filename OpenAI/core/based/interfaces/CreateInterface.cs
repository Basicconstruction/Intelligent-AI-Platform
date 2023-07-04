using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OpenAI.core.models.completion;
using OpenAI.core.models.completion.stream;

namespace OpenAI.core.based.interfaces
{
    public interface ICreateInterface
    {
        Task<OpenAiCompletionModel> Create(
            string model, [Optional] dynamic prompt, [Optional] string suffix, 
            [Optional] int? maxTokens, [Optional] double? temperature, 
            [Optional] double? topP, [Optional] int? n,
            [Optional] int? logprobs, [Optional] bool? echo, [Optional] string stop,
            [Optional] double? presencePenalty, [Optional] double? frequencyPenalty,
            [Optional] int? bestOf, [Optional] Dictionary<string, dynamic> logitBias,
            [Optional] string user, [Optional] HttpClient client
                );

        Task<IObservable<OpenAiStreamCompletionModel>> CreateStream(
            string model, [Optional] dynamic prompt, [Optional] string suffix, 
            [Optional] int? maxTokens, [Optional] double? temperature, 
            [Optional] double? topP, [Optional] int? n,
            [Optional] int? logprobs, [Optional] bool? echo, [Optional] string stop,
            [Optional] double? presencePenalty, [Optional] double? frequencyPenalty,
            [Optional] int? bestOf, [Optional] Dictionary<string, dynamic> logitBias,
            [Optional] string user, [Optional] HttpClient client
            );

        Task<IObservable<string>> CreateStreamText(
            string model, [Optional] dynamic prompt, [Optional] string suffix, 
            [Optional] int? maxTokens, [Optional] double? temperature, 
            [Optional] double? topP, [Optional] int? n,
            [Optional] int? logprobs, [Optional] bool? echo, [Optional] string stop,
            [Optional] double? presencePenalty, [Optional] double? frequencyPenalty,
            [Optional] int? bestOf, [Optional] Dictionary<string, dynamic> logitBias,
            [Optional] string user, [Optional] HttpClient client
            );

    }
}