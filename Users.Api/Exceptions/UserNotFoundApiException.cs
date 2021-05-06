using Microsoft.AspNetCore.Http;

namespace Users.Api.Exceptions
{
    public class UserNotFoundApiException : ApiException<string>
    {
        public UserNotFoundApiException(string message) : base(message)
        {
        }

        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}