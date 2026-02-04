using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidModuleNameException(int MinCharactersNumber, int MaxCharactersNumber) : BaseIamCoreException($"Nazwa modułu może zawierać od {MinCharactersNumber} do {MaxCharactersNumber} znaków.", "001")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}