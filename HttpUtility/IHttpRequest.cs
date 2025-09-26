
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtility
{
    public interface IHttpRequest
    {
        //public Task<HttpResponseMessage> GetAsync(string url);
        public Task<HttpResponse<TResult>> GetAsync<TResult>(string url, Dictionary<string, string>? urlParam = null);
        //public Task<HttpResponseMessage> PostAsync(string url, object input);
        public Task<HttpResponse<TResult>> PostAsync<TResult>(string url, object input, Dictionary<string, string>? urlParam = null);
        //public Task<TResult> PostAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string>? urlParam = null);
        public Task<HttpResponse<TResult>> PostAsync<TResult>(string url, MultipartContent multipartContent, Dictionary<string, string>? urlParam = null);

        //public Task<TResult> PatchAsync<TResult>(string url, MultipartFormDataContent input, Dictionary<string, string>? urlParam = null);
        //public Task<TResult> PatchAsync<TResult>(string url, object input, Dictionary<string, string>? urlParam = null);
        //public Task<string> PutAsync(string url, object input);
        //public Task<TResult> PutAsync<TResult>(string url, object input, Dictionary<string, string>? urlParam = null);
        //public Task<string> PutAsync(string url, HttpContent content);
        public  Task<HttpResponse<string>> DeleteAsync(string url, Dictionary<string, string> urlParam = null);

        public String BaseUrl { set; }
        public String Token { set; get; }
        public bool EnabledProxy { get; set; }
    }
}
