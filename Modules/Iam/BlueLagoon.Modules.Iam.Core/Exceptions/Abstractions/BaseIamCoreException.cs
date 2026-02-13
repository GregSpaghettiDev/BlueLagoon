using BlueLagoon.Shared.Infrastructure.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;

public abstract class BaseIamCoreException : BaseCoreException
{
    public BaseIamCoreException(string message, string errorCode)
        : base(message, errorCode)
    {
    
    }

    public BaseIamCoreException(string message, string errorCode, Exception innerException)
        : base(message, errorCode, innerException)
    {
    }

    public override HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;
}
