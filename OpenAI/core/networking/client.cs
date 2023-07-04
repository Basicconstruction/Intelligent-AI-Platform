using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenAI.core.builder;
using OpenAI.core.constants;

namespace OpenAI.core.networking
{
    public class OpenAiNetworkingClient
    {
        static async Task<dynamic> Get<T>(
            string from,
            Func<Dictionary<string, dynamic>, T> onSuccess,
            [Optional] HttpClient client,
            bool returnRawResponse = false
        )
        {
            var headers = HeadersBuilder.Build();
            client = client ?? new HttpClient();
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            List<byte> responseContent;
            var response = await client.GetAsync(from);
            response.EnsureSuccessStatusCode();
            responseContent = (await response.Content.ReadAsByteArrayAsync()).ToList();

            if (returnRawResponse)
            {
                return responseContent;
            }

            var utf8decoder = new UTF8Encoding();
            var convertBody = utf8decoder.GetString(responseContent.ToArray());
            var decodedBody = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(convertBody);
            if (DoesErrorExists(decodedBody))
            {
                Dictionary<string, dynamic> error =
                    decodedBody[OpenAiStrings.ErrorFieldKey];
                string message = error[OpenAiStrings.MessageFieldKey];
                var statusCode = response.StatusCode + "";
                throw new Exception(message + ": " + statusCode);
            }

            Debug.Assert(onSuccess != null, nameof(onSuccess) + " != null");
            return onSuccess(decodedBody);
        }

        static bool DoesErrorExists(Dictionary<string, dynamic> decodedResponseBody)
        {
            return decodedResponseBody[OpenAiStrings.ErrorFieldKey] != null;
        }

        public static async Task<IObservable<T>> GetStream<T>(
            string from,
            Func<Dictionary<string, dynamic>, T> onSuccess,
            [Optional] HttpClient client
        )
        {
            client = client ?? new HttpClient();
            //var response = await client.GetAsync(from,HttpCompletionOption.ResponseHeadersRead);
            var request = new HttpRequestMessage(HttpMethod.Get, from);
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            var contentStream = await response.Content.ReadAsStreamAsync();
            var streamReader = new StreamReader(contentStream, Encoding.UTF8);

            void Close()
            {
                streamReader.Close();
                contentStream.Close();
            }

            var rxStream = Observable.Create<T>(async (observer) =>
            {
                while (!streamReader.EndOfStream)
                {
                    string line = null;
                    try
                    {
                        line = await streamReader.ReadLineAsync();
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                        observer.OnCompleted();
                        return;
                    }

                    if (line.Trim() != "")
                    {
                        if (line.StartsWith(OpenAiStrings.StreamResponseStart))
                        {
                            var data = line.Substring(6);
                            if (data.StartsWith(OpenAiStrings.StreamResponseEnd))
                            {
                                return;
                            }

                            var decoded = DecodeToMap(data);
                            observer.OnNext(onSuccess(decoded));
                        }
                    }

                }
                observer.OnCompleted();
                Close();
            });
            return rxStream;
        }

        public static async Task<T> Post<T>(
            string to,
            Func<Dictionary<string, dynamic>, T> onSuccess,
            [Optional] Dictionary<string, dynamic> body,
            [Optional] HttpClient client
        )
        {
            client = client ?? new HttpClient();
            var headers = HeadersBuilder.Build();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, to);
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            request.Content = jsonContent;
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            var contentStream = await response.Content.ReadAsStreamAsync();
            var streamReader = new StreamReader(contentStream, Encoding.UTF8);
            var responseBody = await streamReader.ReadToEndAsync();

            void Close()
            {
                streamReader.Close();
                contentStream.Close();
            }

            var decodedBody = DecodeToMap(responseBody);
            if (DoesErrorExists(decodedBody))
            {
                var error = decodedBody[OpenAiStrings.ErrorFieldKey];
                var message = error[OpenAiStrings.MessageFieldKey];
                var statusCode = response.StatusCode;
                Close();
                throw new Exception(JsonConvert.SerializeObject(message) + statusCode);
            }

            Close();
            return onSuccess(decodedBody);
        }

        public static async Task<IObservable<T>> PostStream<T>(
            string to, Dictionary<string, dynamic> body,
            Func<Dictionary<string, dynamic>, T> onSuccess,
            [Optional] HttpClient client)
        {
            client = client ?? new HttpClient();
            var headers = HeadersBuilder.Build();
            var serialBody = JsonConvert.SerializeObject(body);
            var jsonContent = new StringContent(serialBody, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, to);
            request.Content = jsonContent;
            foreach (var header in headers)
            {
                if(header.Key.ToLower().StartsWith("au"))
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            var contentStream = await response.Content.ReadAsStreamAsync();
            var streamReader = new StreamReader(contentStream, Encoding.UTF8);

            void Close()
            {
                streamReader.Close();
                contentStream.Close();
            }

            var rxStream = Observable.Create<T>(async (observer) =>
            {
                while (!streamReader.EndOfStream)
                {
                    string line = null;
                    try
                    {
                        line = await streamReader.ReadLineAsync();
                        Console.WriteLine(line);
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                        observer.OnCompleted();
                        return;
                    }

                    if (line.Trim() != "")
                    {
                        if (line.StartsWith(OpenAiStrings.StreamResponseStart))
                        {
                            var data = line.Substring(6);
                            if (data.StartsWith(OpenAiStrings.StreamResponseEnd))
                            {
                                observer.OnCompleted();
                                Close();
                            }
                            else
                            {
                                var decoded = DecodeToMap(data);
                                observer.OnNext(onSuccess(decoded));
                            }
                        }
                        else
                        {
                            observer.OnError(new Exception(line));   
                        }

                    }
                }
                observer.OnCompleted();
                Close();
            });
            return rxStream;
        }

        static Dictionary<string, dynamic> DecodeToMap(string responseBody)
        {
            try
            {
                // 未验证
                return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(responseBody);
            }
            catch (Exception e)
            {
                return null;
                throw new Exception(e.ToString());
            }
        }

        public static async Task<Stream> NewStream(string to, Dictionary<string,dynamic> body)
        {
            var client = new HttpClient();
            var headers = HeadersBuilder.Build();
            var serialBody = JsonConvert.SerializeObject(body);
            var jsonContent = new StringContent(serialBody, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, to);
            request.Content = jsonContent;
            foreach (var header in headers)
            {
                if(header.Key.ToLower().StartsWith("au"))
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();
        }
    }
}