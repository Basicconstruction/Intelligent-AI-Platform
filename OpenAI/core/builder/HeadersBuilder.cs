using System.Collections.Generic;
using System.Diagnostics;

namespace OpenAI.core.builder
{
    public class HeadersBuilder
    {
        private static string _apiKey;
        private static string _organization;
        private static Dictionary<string, dynamic> _additionalHeadersToRequests = new Dictionary<string, dynamic>();
        public static string Organization
        {
            get => _organization;
            set => _organization = value;
        }
        public static bool IsOrganizationSet => Organization != null&&Organization!="";

        public static string ApiKey
        {
            get => _apiKey;
            set => _apiKey = value;
        }

        public static Dictionary<string, string> Build()
        {
            var headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            Debug.Assert(_apiKey != null, "You must set the API key before making building any headers for a request.");
            foreach (var additionalHeaders in _additionalHeadersToRequests)
            {
                headers.Add(additionalHeaders.Key,additionalHeaders.Value);
            }

            if (IsOrganizationSet)
            {
                headers.Add("OpenAI-Organization",_organization);
            }
            headers.Add("Authorization","Bearer "+_apiKey);
            return headers;
        }

        public static void IncludeHeaders(Dictionary<string, dynamic> headers)
        {
            foreach (var header in headers)
            {
                _additionalHeadersToRequests.Add(header.Key,header.Value);
            }
        }


    }
}