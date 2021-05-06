using System;

namespace Users.Api.Exceptions
{
    public abstract class ApiException : Exception
    {
        public abstract int StatusCode { get; }
    }

    public abstract class ApiException<T> : ApiException
    {
        protected ApiException(T errorData) => ErrorData = errorData;
        public T ErrorData { get; set; }
    }
}