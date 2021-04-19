using Microsoft.AspNetCore.Http;

namespace Tasks.Api.Exceptions
{
    public class AccessRightApiException : ApiException
    {
        public AccessRightApiException(string message) : base(message)
        {
        }

        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}