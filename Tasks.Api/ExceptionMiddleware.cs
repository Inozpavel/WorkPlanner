using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasks.Api.Exceptions;

namespace Tasks.Api
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiException e)
            {
                context.Response.StatusCode = e.StatusCode;

                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Detail = e.Message,
                    Status = e.StatusCode,
                });
            }
        }
    }
}