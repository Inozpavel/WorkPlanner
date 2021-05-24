using Microsoft.AspNetCore.Http;

namespace Tasks.Api.Exceptions
{
    public class AlreadyDoneApiException : ApiException
    {
        public AlreadyDoneApiException(string message) : base(message)
        {
        }

        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}