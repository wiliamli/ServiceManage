using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Jwell.Framework.Extensions;
using System.Threading.Tasks;

namespace Jwell.Modules.DSFClient
{
    internal static class ApiClient
    {
        private static HttpClient HttpClient { get; }

        static ApiClient()
        {
            HttpClient = new HttpClient();
        }

        public static void SetHttpClient(string address)
        {
            HttpClient.BaseAddress = new Uri(address);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        internal static string Request<T>(string url, MethodEnum method, T value = default(T))
        {
            var response = string.Empty;
            if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    switch (method.EnumDesc().ToUpper(System.Globalization.CultureInfo.CurrentCulture))
                    {
                        case "GET":
                            response = GetAsync(url).Result;                           
                            break;
                        case "POST":
                            response = PostAsync(url, value).Result;                           
                            break;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            else
            {
                throw new Exception("url不能为空");
            }

            return response;
        }

        //private static string Get(string url)
        //{
        //    var meesage = HttpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead).Result;
        //    var body = meesage.Content.ReadAsStringAsync().Result;
        //    return body;
        //}

        //private static string Post<T>(string url,T value)
        //{
        //    var meesage = HttpClient.PostAsJsonAsync(url, value).Result;
        //    var body = meesage.Content.ReadAsStringAsync().Result;
        //    return body;
        //}

        private static async Task<string> GetAsync(string url)
        {
            var content = await HttpClient.GetAsync(url,HttpCompletionOption.ResponseContentRead);

            var body = await content.Content.ReadAsStringAsync();

            return body;
        }

        private static async Task<string> PostAsync<T>(string url, T value)
        {
            var content = await HttpClient.PostAsJsonAsync(url, value);

            var body = await content.Content.ReadAsStringAsync();

            return body;
        }
    }
}
