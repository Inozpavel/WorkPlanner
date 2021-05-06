using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Users.Api.Exceptions
{
    public class RegistrationException : ApiException<ValidationProblemDetails>
    {
        public RegistrationException(ValidationProblemDetails problemDetails) : base(problemDetails)
        {
        }

        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}