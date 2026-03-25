using System.Net;

namespace Ecosystem.AccountService.Domain.Exceptions;

public class BaseException : Exception
{
    protected BaseException() { }

    protected BaseException(string message) : base(message) { }

    protected BaseException(string format, params object[] args) : base(string.Format(format, args)) { }

    protected BaseException(string message, Exception innerException) : base(message, innerException) { }

    protected BaseException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }


    protected BaseException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    protected BaseException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    protected BaseException(string format, HttpStatusCode statusCode, params object[] args) : base(string.Format(format, args))
    {
        StatusCode = statusCode;
    }

    protected BaseException(string message, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    protected BaseException(string format, HttpStatusCode statusCode, Exception innerException, params object[] args) : base(string.Format(format, args), innerException)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; set; }
}