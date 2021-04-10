using System;
using Microsoft.AspNetCore.Http;

namespace Tasks.Api.Exceptions
{
    [Serializable]
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}