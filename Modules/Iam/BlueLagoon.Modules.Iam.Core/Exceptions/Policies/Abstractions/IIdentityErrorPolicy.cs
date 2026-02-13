using BlueLagoon.Shared.Infrastructure.Exceptions.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace BlueLagoon.Modules.Iam.Core.Exceptions.Policies.Abstractions;

internal interface IIdentityErrorPolicy
{
    bool CanHandle(string code);

    BaseCoreException Handle(IdentityError identityError, bool whetherReturnException = false);
}
