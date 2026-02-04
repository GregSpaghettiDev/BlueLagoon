using System.Net;

namespace BlueLagoon.Shared.Infrastructure.Exceptions.Abstractions;

public abstract class BaseApplicationException : Exception
{
    public string ErrorCode { get; init; }

    public abstract HttpStatusCode StatusCode { get; }
}
