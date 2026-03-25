using System.Net;

namespace Ecosystem.AccountService.Domain.Exceptions;

public abstract class CustomException : BaseException
{
    protected CustomException() { }

    protected CustomException(string message) : base(message) { }

    protected CustomException(string message, Exception innerException) : base(message, innerException) { }

    protected CustomException(HttpStatusCode statusCode, string exceptionBody)
    {
        StatusCode = statusCode;
        ExceptionBody = exceptionBody;
    }

    public string? ExceptionBody { get; set; }
}