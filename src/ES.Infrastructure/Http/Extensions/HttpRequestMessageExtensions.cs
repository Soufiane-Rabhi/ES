using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ES.Infrastructure.Serialization;
using Microsoft.AspNetCore.WebUtilities;

namespace ES.Http.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static async Task<HttpResponseMessage> PostAsJsonAsync<TModel>(this HttpClient client, string requestUrl, TModel model, IJsonSerializer jsonSerializer)
        {
            var json = jsonSerializer.Serialize(model);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await client.PostAsync(requestUrl, stringContent);
        }

        public static HttpRequestMessage Get(this HttpRequestMessage request)
        {
            request.Method = HttpMethod.Get;

            return request;
        }

        public static HttpRequestMessage Post(this HttpRequestMessage request)
        {
            request.Method = HttpMethod.Post;

            return request;
        }

        public static HttpRequestMessage ForUrl(this HttpRequestMessage request, string requestUri)
        {
            request.RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute);

            return request;
        }

        public static HttpRequestMessage ForUrlWithQueryString(this HttpRequestMessage request, string requestUri, IDictionary<string, string> queryString)
        {
            var query = QueryHelpers.AddQueryString(requestUri, queryString);

            request.RequestUri = new Uri(query, UriKind.RelativeOrAbsolute);

            return request;
        }

        public static HttpRequestMessage WithBearer(this HttpRequestMessage request, string token)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
           
            return request;
        }

        public static HttpRequestMessage WithJsonPayload<T>(this HttpRequestMessage request, T payload, IJsonSerializer jsonSerializer, SerializerSettings serializerSettings)
        {
            var json = jsonSerializer.Serialize(payload, serializerSettings);
            request.Content = new StringContent(json);

            return request;
        }

        public static HttpRequestMessage WithJsonPayload<T>(this HttpRequestMessage request, T payload, IJsonSerializer jsonSerializer)
        {
            var json = jsonSerializer.Serialize(payload);
            request.Content = new StringContent(json,Encoding.UTF8, "application/json");

            return request;
        }

        public static HttpRequestMessage WithFile(this HttpRequestMessage request, byte[] data, string fileName)
        {
            request.Content = new MultipartFormDataContent
            {
                { new ByteArrayContent(data), "file", fileName },
                { new StringContent(Path.GetFileNameWithoutExtension(fileName)), "\"name\"" },
                { new StringContent("xml"), "\"file_type\"" }
            };

            return request;
        }

    }
}
