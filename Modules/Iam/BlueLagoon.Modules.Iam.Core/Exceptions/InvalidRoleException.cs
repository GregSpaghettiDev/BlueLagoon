using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidRoleException(int minCharactersNumber, int maxCharactersNumber) 
    : BaseIamCoreException($"Kod, nazwa roli mogą posiadać od {minCharactersNumber} do {maxCharactersNumber} znaków.", "007")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}