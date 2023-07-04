using OpenAI.core.constants;

namespace OpenAI.core.builder
{
    public abstract class BaseApiUrlBuilder
    {
        public static string Build(string endpoint, string id=null, string query=null)
        {
            var baseUrl = OpenAiConfig.BaseUrl;
            var version = OpenAiConfig.Version;
            var usedEndpoint = HandleEndpointsStarting(endpoint);
            var apiLink = baseUrl;
            apiLink += "/" + version;
            apiLink += usedEndpoint;
            if (id != null)
            {
                apiLink += "/" + id;
            }else if (query != null)
            {
                apiLink += "?" + query;
            }

            return apiLink;
        }
        private static string HandleEndpointsStarting(string endpoint) {
            return endpoint.StartsWith("/") ? endpoint : "/$endpoint";
        }
    }
}