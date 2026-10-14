using HttpUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtility
{
    public class HttpRequest : IHttpRequest
    {

        private string _baseURL;
        public string BaseUrl { get => _baseURL; set => _baseURL = !value.EndsWith("/") ? value + "/" : value; }
        public string Token { get; set; }
        public bool EnabledProxy { get; set; }

        private HttpClient _httpclient;
        BaseHttpHandler BaseHttpHandler { get; set; }
        InterceptorAsync preSendAsync;
        public HttpRequest(bool IsProxy = false, InterceptorAsync preSendAsync = null)
        {
            this.preSendAsync = preSendAsync;   
            BaseHttpHandler = new BaseHttpHandler(IsProxy, preSendAsync);
        }
        private void CreateHttpClient()
        {
            _httpclient = new HttpClient(BaseHttpHandler);
            _httpclient.BaseAddress = new Uri(BaseUrl);
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }
        private void IsSameBaseURL()
        {
            if (_httpclient == null || _httpclient.BaseAddress.ToString() != BaseUrl)
                CreateHttpClient();
        }
        private async Task<HttpResponseMessage> GetAsync(string url)
        {

            return await _httpclient.GetAsync(url);

        }

        public async Task<HttpResponse<TResult>> GetAsync<TResult>(string url, Dictionary<string, string> urlParam = null)
        {
            IsSameBaseURL();
            if (urlParam != null)
                url = AttachUrlParam(url, urlParam);
            HttpResponseMessage response = await GetAsync(url);
            // 讀取回應內容
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return new HttpResponse<TResult>(response.StatusCode, responseBody);
            }

            TResult result = JsonConvert.DeserializeObject<TResult>(responseBody);
            return new HttpResponse<TResult>(result);

        }
        private string AttachUrlParam(string url, Dictionary<string, string> urlParam)
        {

            url += "?";
            url += string.Join("&", urlParam.Select(x => $"{x.Key}={x.Value}"));

            return url;
        }
        //public async Task PostAsync(HttpContent content,  string url)
        //{
        //        HttpResponseMessage response = await _httpclient.PostAsync(url, content);
        //        string responseBody = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine(responseBody);          
        //}

        private async Task<HttpResponseMessage> PostAsync(string url, object input)
        {
            StringContent content = null;
            if (input != null)
            {
                string json = JsonConvert.SerializeObject(input);              
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            var response = await _httpclient.PostAsync(url, content);
            return response;
        }

        public async Task<HttpResponse<TResult>> PostAsync<TResult>(string url, object input = null, Dictionary<string, string> urlParam = null)
        {
            IsSameBaseURL();
            if (urlParam != null)
                url = AttachUrlParam(url, urlParam);
            HttpResponseMessage response = await PostAsync(url, input);
            //response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return new HttpResponse<TResult>(response.StatusCode, responseBody);
            }

            TResult result = JsonConvert.DeserializeObject<TResult>(responseBody);
            return new HttpResponse<TResult>(result);
        }

        public async Task<HttpResponse<TResult>> PostAsync<TResult>(string url, MultipartFormDataContent multipartContent, Dictionary<string, string> urlParam = null)
        {
            IsSameBaseURL();
            if (urlParam != null)
                url = AttachUrlParam(url, urlParam);
            HttpResponseMessage response = new HttpResponseMessage();
            //StringContent content = input == null ? null : new StringContent(JsonConvert.SerializeObject(input));

            response = await _httpclient.PostAsync(url, multipartContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return new HttpResponse<TResult>(response.StatusCode, responseBody);
            }

            TResult result = JsonConvert.DeserializeObject<TResult>(responseBody);
            return new HttpResponse<TResult>(result);
        }

        public async Task<HttpResponse<TResult>> PostAsync<TResult>(string url, MultipartContent multipartContent, Dictionary<string, string> urlParam = null)
        {
            IsSameBaseURL();
            if (urlParam != null)
                url = AttachUrlParam(url, urlParam);
            HttpResponseMessage response = new HttpResponseMessage();
            //StringContent content = input == null ? null : new StringContent(JsonConvert.SerializeObject(input));

            response = await _httpclient.PostAsync(url, multipartContent);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return new HttpResponse<TResult>(response.StatusCode, responseBody);
            }

            TResult result = JsonConvert.DeserializeObject<TResult>(responseBody);
            return new HttpResponse<TResult>(result);


        }

        public async Task<HttpResponse<string>> DeleteAsync(string url, Dictionary<string, string> urlParam = null)
        {
            IsSameBaseURL();
            if (urlParam != null)
                url = AttachUrlParam(url, urlParam);
            using (HttpResponseMessage response = await _httpclient.DeleteAsync(url))
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return new HttpResponse<string>(response.StatusCode, responseBody);
            }
        }
        public async Task<HttpResponseMessage> PutAsync(string url, object input)
        {
            StringContent content = null;
            if (input != null)
            {
                string json = JsonConvert.SerializeObject(input);
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            var response = await _httpclient.PutAsync(url, content);
            return response;
        }
        public async Task<HttpResponse<TResult>> PutAsync<TResult>(string url, object input = null, Dictionary<string, string> urlParam = null)
        {
            IsSameBaseURL();
            if (urlParam != null)
                url = AttachUrlParam(url, urlParam);
            HttpResponseMessage response = await PutAsync(url, input);
            //response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return new HttpResponse<TResult>(response.StatusCode, responseBody);
            }

            TResult result = JsonConvert.DeserializeObject<TResult>(responseBody);
            return new HttpResponse<TResult>(result);
        }
    }
}
