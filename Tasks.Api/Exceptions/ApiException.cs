using System;

namespace Tasks.Api.Exceptions
{
    [Serializable]
    public abstract class ApiException : Exception
    {
        protected ApiException()
        {
        }

        protected ApiException(string message) : base(message)
        {
        }

        protected ApiException(string message, Exception inner) : base(message, inner)
        {
        }

        public abstract int StatusCode { get; }
    }
}