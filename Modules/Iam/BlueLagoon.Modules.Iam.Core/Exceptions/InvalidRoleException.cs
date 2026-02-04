using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidRoleException(int MinCharactersNumber, int MaxCharactersNumber) : BaseIamCoreException($"Niepoprawna rola. Rola może zawierać od {MinCharactersNumber} do {MaxCharactersNumber} znaków.", "007")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}