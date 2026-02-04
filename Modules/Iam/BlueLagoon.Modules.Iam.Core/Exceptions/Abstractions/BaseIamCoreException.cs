using BlueLagoon.Shared.Infrastructure.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;

public abstract class BaseIamCoreException(string Message, string ErrorCode) : BaseCoreException(Message, $"IAM_B{ErrorCode}")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;
}
