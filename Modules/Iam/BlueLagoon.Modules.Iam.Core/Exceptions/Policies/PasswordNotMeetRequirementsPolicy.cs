using BlueLagoon.Modules.Iam.Core.Exceptions.Policies.Abstractions;
using BlueLagoon.Shared.Infrastructure.Exceptions.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace BlueLagoon.Modules.Iam.Core.Exceptions.Policies;

internal sealed class PasswordNotMeetRequirementsPolicy : IIdentityErrorPolicy
{
    public bool CanHandle(string code)
        => code is nameof(IdentityErrorDescriber.PasswordMismatch)
                or nameof(IdentityErrorDescriber.PasswordTooShort)
                or nameof(IdentityErrorDescriber.PasswordRequiresLower)
                or nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric)
                or nameof(IdentityErrorDescriber.PasswordRequiresLower)
                or nameof(IdentityErrorDescriber.PasswordRequiresUpper)
                or nameof(IdentityErrorDescriber.PasswordRequiresUniqueChars)
                or nameof(IdentityErrorDescriber.PasswordRequiresDigit);

    public BaseCoreException Handle(IdentityError identityError, bool whetherReturnException = false)
    {
        var exception = new PasswordNotMeetRequirementsException(identityError.Code, identityError.Description);

        if (whetherReturnException)
            return exception;

        throw exception;
    }
}