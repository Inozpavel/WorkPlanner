using Microsoft.AspNetCore.Http;

namespace Tasks.Api.Exceptions
{
    public class AccessRightException : ApiException
    {
        public AccessRightException(string message) : base(message)
        {
        }

        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}