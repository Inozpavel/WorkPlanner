using System;

namespace Tasks.Api.Exceptions
{
    [Serializable]
    public abstract class ApiException : Exception
    {
        protected ApiException(string message) : base(message)
        {
        }

        public abstract int StatusCode { get; }
    }
}