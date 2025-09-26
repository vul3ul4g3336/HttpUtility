using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtility
{
    public class HttpResponse<T> 
    {
        public HttpStatusCode Code { get; set; } = HttpStatusCode.OK;
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        public T Data { get; set; } = default;
        
        public HttpResponse(HttpStatusCode code, bool isSuccess)
        {
            this.Code = code;
            this.IsSuccess = isSuccess;
        }

        public HttpResponse(HttpStatusCode code,string message)
        {
            this.IsSuccess = false;
            this.Code = code;
            this.Message = message;
        }

        public HttpResponse(HttpStatusCode code, bool isSuccess, string message)
        {
            this.Code = code;
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        public HttpResponse(T data)
        {
            this.Data = data;
        }

    }
}
