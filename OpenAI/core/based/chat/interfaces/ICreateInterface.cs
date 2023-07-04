using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OpenAI.core.models.chat;
using OpenAI.core.models.chat.stream;
using OpenAI.core.models.chat.submodels.choices.submodels;

namespace OpenAI.core.based.chat.interfaces
{
    public interface ICreateInterface
    {
        Task<OpenAiChatCompletionModel> Create(
            string model,
            List<OpenAiChatCompletionChoiceMessageModel> messages,
            [Optional] double temperature,
            [Optional] double topP,
            [Optional] int n,
            [Optional] dynamic stop,
            [Optional] int maxTokens,
            [Optional] double presencePenalty,
            [Optional] double frequencyPenalty,
            [Optional] Dictionary<string,dynamic> logitBias,
            [Optional] string user,
            [Optional] HttpClient client);
        Task<IObservable<OpenAiStreamChatCompletionModel>> CreateStream(
            string model,
            List<OpenAiChatCompletionChoiceMessageModel> messages,
            [Optional] double temperature,
            [Optional] double topP,
            [Optional] int n,
            [Optional] dynamic stop,
            [Optional] int maxTokens,
            [Optional] double presencePenalty,
            [Optional] double frequencyPenalty,
            [Optional] Dictionary<string,dynamic> logitBias,
            [Optional] string user,
            [Optional] HttpClient client
            );
    }
}