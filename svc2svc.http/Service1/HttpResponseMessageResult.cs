using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service1
{
    public class HttpResponseMessageResult : IActionResult
    {
        private readonly HttpResponseMessage responseMessage;

        public HttpResponseMessageResult(HttpResponseMessage responseMessage)
        {
            this.responseMessage = responseMessage; // could add throw if null
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;

            if (responseMessage == null)
            {
                var message = "Response message cannot be null";

                throw new InvalidOperationException(message);
            }

            using (responseMessage)
            {
                response.StatusCode = (int)responseMessage.StatusCode;

                var responseFeature = context.HttpContext.Features.Get<IHttpResponseFeature>();
                if (responseFeature != null)
                {
                    responseFeature.ReasonPhrase = responseMessage.ReasonPhrase;
                }

                var responseHeaders = responseMessage.Headers;

                // Ignore the Transfer-Encoding header if it is just "chunked".
                // We let the host decide about whether the response should be chunked or not.
                if (responseHeaders.TransferEncodingChunked == true &&
                    responseHeaders.TransferEncoding.Count == 1)
                {
                    responseHeaders.TransferEncoding.Clear();
                }

                foreach (var header in responseHeaders)
                {
                    response.Headers.Append(header.Key, header.Value.ToArray());
                }

                if (responseMessage.Content != null)
                {
                    var contentHeaders = responseMessage.Content.Headers;

                    // Copy the response content headers only after ensuring they are complete.
                    // We ask for Content-Length first because HttpContent lazily computes this
                    // and only afterwards writes the value into the content headers.
                    var unused = contentHeaders.ContentLength;

                    foreach (var header in contentHeaders)
                    {
                        response.Headers.Append(header.Key, header.Value.ToArray());
                    }

                    await responseMessage.Content.CopyToAsync(response.Body);
                }
            }
        }
    }
}