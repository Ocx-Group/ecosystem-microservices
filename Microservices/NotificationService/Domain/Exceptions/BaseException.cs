using System.Net;

namespace Ecosystem.NotificationService.Domain.Exceptions;

public class BaseException : Exception
{
    protected BaseException() { }

    protected BaseException(string message) : base(message) { }

    protected BaseException(string format, params object[] args) : base(string.Format(format, args)) { }

    protected BaseException(string message, Exception innerException) : base(message, innerException) { }

    protected BaseException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    protected BaseException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; set; }
}
