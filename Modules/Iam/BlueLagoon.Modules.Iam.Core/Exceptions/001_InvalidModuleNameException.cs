using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidModuleNameException(int minCharactersNumber, int maxCharactersNumber) 
    : BaseIamCoreException($"Nazwa modułu może zawierać od {minCharactersNumber} do {maxCharactersNumber} znaków.", "001")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}