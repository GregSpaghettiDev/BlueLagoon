using System.Net;

namespace BlueLagoon.Shared.Infrastructure.Exceptions.Abstractions;

public abstract class BaseCoreException : Exception
{
    public BaseCoreException(string message, string errorCode)
        : base(message)
    {
        this.ErrorCode = errorCode;
    }

    public BaseCoreException(string message, string errorCode, Exception innerException)
        : base(message, innerException)
    {
        this.ErrorCode = errorCode;
    }

    public string ErrorCode { get; init; }

    public abstract HttpStatusCode StatusCode { get; }
}
