using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpUtility
{

    public delegate Task<Interceptor> InterceptorAsync(
        HttpRequestMessage request, CancellationToken ct,
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendDownstream
        );

    public class BaseHttpHandler : DelegatingHandler
    {
        public InterceptorAsync? InterceptorAsync { get; }
        public BaseHttpHandler(bool isEnabledProxy = false, InterceptorAsync interceptorAsync = null)
        {
           
            var handler =  new HttpClientHandler();
            if (isEnabledProxy)
            {
                var proxy = new WebProxy("http://127.0.0.1:8888", false);
                handler.Proxy = proxy;
                handler.UseProxy = true;
                // 如果需要攔截 HTTPS，要允許忽略憑證錯誤 (因為 Fiddler 會產生自簽憑證)
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            }
            InnerHandler = handler;

            this.InterceptorAsync = interceptorAsync;
        }
       
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            if (InterceptorAsync != null)
            {
                // 下游送件器：只走 InnerHandler 之後的管線（含 Proxy），不回到本 handler（避免遞迴）
                var invoker = new HttpMessageInvoker(InnerHandler!, disposeHandler: false);
                Task<HttpResponseMessage> SendDownstream(HttpRequestMessage r, CancellationToken c)
                    => invoker.SendAsync(r, c);

                Interceptor interceptorResponse = await InterceptorAsync(request, ct, SendDownstream).ConfigureAwait(false);

                if (interceptorResponse.ShortCircuitResponse != null)
                    return interceptorResponse.ShortCircuitResponse;

                if (interceptorResponse.RequestToSend != null)
                    request = interceptorResponse.RequestToSend;  // 用外部替換後的 Request
                // 否則沿用（可能已被就地修改的）原 Request
            }

            // 統一走整條 handler 管線（保留 Proxy/其它 handler）
            return await base.SendAsync(request, ct).ConfigureAwait(false);
        }

    }
}
