using BlueLagoon.Modules.Iam.Core.Exceptions.Policies.Abstractions;
using BlueLagoon.Shared.Infrastructure.Exceptions.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace BlueLagoon.Modules.Iam.Core.Exceptions.Policies;

internal sealed class InvalidUserNamePolicy : IIdentityErrorPolicy
{
    public bool CanHandle(string code)
        => code == nameof(IdentityErrorDescriber.InvalidUserName);

    public BaseCoreException Handle(IdentityError identityError, bool whetherReturnException = false)
    {
        var exception = new InvalidUserNameException(identityError.Code, identityError.Description);

        if (whetherReturnException)
            return exception;

        throw exception;
    }
}