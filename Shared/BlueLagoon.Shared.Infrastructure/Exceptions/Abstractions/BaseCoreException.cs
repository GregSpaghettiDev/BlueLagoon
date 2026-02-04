using System.Net;

namespace BlueLagoon.Shared.Infrastructure.Exceptions.Abstractions;

public abstract class BaseCoreException(string Message, string ErrorCode) : Exception(Message)
{
    public string ErrorCode { get; init; } = ErrorCode;

    public abstract HttpStatusCode StatusCode { get; }
}
