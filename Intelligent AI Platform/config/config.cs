using System;
using System.IO;
using Intelligent_AI_Platform.Model.platform.app.GenericChat.chat;
using Newtonsoft.Json;

namespace Intelligent_AI_Platform.config
{
    public class Configuration
    {
        public double Temperature = 0.3;
        public int MaxTokens = 3000;
        public string Model = "gpt-3.5-turbo";
        public string ProviderUrl = "https://api.openai-sb.com";
        public double RequestRate = 0.6;
        public string Key = "";

        public void TurnsTo(Configuration configuration)
        {
            Temperature = configuration.Temperature;
            MaxTokens = configuration.MaxTokens;
            Model = configuration.Model;
            ProviderUrl = configuration.ProviderUrl;
            RequestRate = configuration.RequestRate;
            Key = configuration.Key;
        }
        public Configuration Copy()
        {
            return new Configuration()
            {
                Temperature = Temperature,
                MaxTokens = MaxTokens,
                Model = Model,
                ProviderUrl = ProviderUrl,
                RequestRate = RequestRate,
                Key = Key
            };
        }
        public static void Serialize(Configuration configuration,string location)
        {
            var app = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var dataPath = app + "//" + location;
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            var data = dataPath + "//" + "app.conf";
            if (!File.Exists(data))
            {
                using (File.Create(data))
                {
                    
                }
            }

            var json = JsonConvert.SerializeObject(configuration);
            File.WriteAllText(data,json);
        }

        public static Configuration LoadConfiguration(string location)
        {
            var app = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var dataPath = app + "//" + location;
            if (!Directory.Exists(dataPath))
            {
                return null;
            }

            var data = dataPath + "//" + "app.conf";
            if (!File.Exists(data))
            {
                return null;
            }

            try
            {
                var res = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(data));
                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}