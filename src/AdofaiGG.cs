using System;
using System.Net.Http;

namespace AdofaiSRM.src
{
    internal class AdofaiGG
    {
        private static readonly HttpClient client = new HttpClient();

        public static HttpResponseMessage getLevelData(string id)
        {
            var levelDataRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://adofai.gg:9200/api/v1/levels/{id}"),
                Headers =
                {
                    { "accept", "application/json" },
                }
            };
            return client.SendAsync(levelDataRequest).Result;
        }
    }
}