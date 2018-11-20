using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Jwell.Framework.Extensions;

namespace Jwell.Modules.DSFClient.BLL
{
    public class ApiClient:IApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(string baseUrl)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };           
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }    

        public string Request<T>(string url, MethodEnum method, T value = default(T))
        {
            var response = string.Empty;

            if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    switch (method.EnumDesc().ToUpper(System.Globalization.CultureInfo.CurrentCulture))
                    {
                        case "GET":
                            //response = GetAsync(url).Result;
                            response = Get(url);
                            break;
                        case "POST":
                            //response = PostAsync(url, value).Result;
                            response = Post(url, value);
                            break;
                        case "PUT":
                            response = Put(url, value);
                            break;
                        case "DELETE":
                            response = Delete(url);
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

        //private async Task<string> GetAsync(string url)
        //{
        //    var content = await _client.GetAsync(url, HttpCompletionOption.ResponseContentRead);

        //    var body = await content.Content.ReadAsStringAsync();

        //    return body;
        //}

        //private async Task<string> PostAsync<T>(string url, T value)
        //{
        //    var content = await _client.PostAsJsonAsync(url, value);

        //    var body = await content.Content.ReadAsStringAsync();

        //    return body;
        //}

        private string Get(string url)
        {
            var meesage = _client.GetAsync(url, HttpCompletionOption.ResponseContentRead).Result;            
            var result = meesage.Content.ReadAsStringAsync().Result;
            return result;
        }

        private string Post<T>(string url, T value)
        {
            var meesage = _client.PostAsJsonAsync(url, value).Result;
            var result = meesage.Content.ReadAsStringAsync().Result;
            return result;
        }

        private string Put<T>(string url, T value)
        {
            var message = _client.PutAsJsonAsync(url, value).Result;
            var result = message.Content.ReadAsStringAsync().Result;
            return result;
        }

        private string Delete(string url)
        {
            var message = _client.DeleteAsync(url).Result;
            var result = message.Content.ReadAsStringAsync().Result;
            return result;
        }
    }
}