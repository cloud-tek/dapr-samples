using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Service1
{
    public static class HttpResponseMessageExtensions
    {
        public static IActionResult AsHttpResponseMessageResult(this HttpResponseMessage response)
        {
            return new HttpResponseMessageResult(response);
        }
    }
}