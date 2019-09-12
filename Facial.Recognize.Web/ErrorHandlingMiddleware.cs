using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Facial.Recognize.Web
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            if (exception is UnauthorizedAccessException)
            {
                code = HttpStatusCode.Unauthorized;
            }

            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new { errorCode = code, error = exception.Message }));
        }
    }
}
