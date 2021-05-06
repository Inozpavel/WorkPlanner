using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Exceptions;

namespace Users.Api
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
            catch (RegistrationException e)
            {
                context.Response.StatusCode = e.StatusCode;

                await context.Response.WriteAsJsonAsync(e.ErrorData);
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