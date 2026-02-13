using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal sealed class PasswordsDoesNotMatchException() : BaseIamCoreException("Podane hasła są od siebie różne.", "021")
{
}