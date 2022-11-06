using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AdofaiSRM.src
{
    internal class VirusScanner
    {
        private string base64;
        private string contentType;
        private string fileName;
        private static readonly HttpClient client = new HttpClient();

        public VirusScanner(string contentType, string fileName, string base64)
        {
            this.contentType = contentType;
            this.fileName = fileName;
            this.base64 = base64;
        }

        public HttpResponseMessage UploadFile()
        {
            var fileUploadRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://www.virustotal.com/api/v3/files"),
                Headers =
                {
                    { "accept", "application/json" },
                    { "x-apikey", Secrets.GetVirusTotalApiKey() },
                },
                Content = new MultipartFormDataContent
                {
                    new StringContent($"")
                    {
                        Headers = {
                            ContentType = new MediaTypeHeaderValue(contentType),
                            ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = "file",
                                FileName = fileName,
                            }
                        }
                    }
                }
            };
            return client.SendAsync(fileUploadRequest).Result;
        }

        public HttpResponseMessage getAnalysisResults(string analysisId)
        {
            Console.WriteLine("Getting analysis");
            var analysisRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://www.virustotal.com/api/v3/files/{analysisId}"),
                Headers = {
                    { "accept", "application/json" },
                    { "x-apikey", Secrets.GetVirusTotalApiKey() },
                }
            };
            Console.WriteLine("Sending analysis");
            return client.SendAsync(analysisRequest).Result;
        }
    }
}