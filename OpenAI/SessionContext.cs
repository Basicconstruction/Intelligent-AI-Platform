using System;
using System.Collections.Generic;
using System.Linq;
using OpenAI.core.models.chat.submodels.choices.submodels;
using OpenAI.instance;

namespace OpenAI
{
    public class SessionContext
    {
        private readonly List<Talk> _talks = new List<Talk>();

        public List<Talk> Talks => _talks;

        public void AddContext(Talk talk)
        {
            if (_talks.FirstOrDefault(t => t.Time == talk.Time) == null)
            {
                _talks.Add(talk);
            }
        }

        public void Clear()
        {
            _talks.Clear();
        }

        public void AddContext(List<Talk> talks)
        {
            foreach (var talk in talks)
            {
                if (_talks.FirstOrDefault(t => t.Time == talk.Time) == null)
                {
                    _talks.Add(talk);
                }
            }
        }

        public void RemoveContext(Talk talk)
        {
            if (_talks.FirstOrDefault(t => t.Time == talk.Time) != null)
            {
                _talks.Remove(talk);
            }
        }

        public void RemoveContext(List<Talk> talks)
        {
            foreach (var talk in talks)
            {
                if (_talks.FirstOrDefault(t => t.Time == talk.Time) != null)
                {
                    _talks.Remove(talk);
                }
            }
        }

        public List<OpenAiChatCompletionChoiceMessageModel> Build(int maxTokens)
        {
            var inputTokenLimit = (int)(OpenAi.InputTokenLimit*maxTokens);
            var current = 0;
            var output = new List<OpenAiChatCompletionChoiceMessageModel>();
            var it = Talks.Count-1;
            while (current <= inputTokenLimit&&it>=0)
            {
                var least = inputTokenLimit - current;
                var talk = Talks[it];
                var nextToken = CalculateTokens(talk.Content);
                if (nextToken < least)
                {
                    output.Insert(0,new OpenAiChatCompletionChoiceMessageModel(
                        role: OpenAiChatCompletionChoiceMessageModel.Parse(talk.Participant.ToString()),
                        talk.Content
                        ));
                    current += nextToken;
                }
                else
                {
                    break;
                }

                it--;
            }

            return output;
        }

        private static int CalculateTokens(string content)
        {
            var res = 0;
            foreach (var c in content)
            {
                if (Convert.ToInt32(c) < 128)
                {
                    res += 1;
                }
                else
                {
                    res += 2;
                }
            }

            return res;
        }
    }
}