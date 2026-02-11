using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal class InvalidPermissionNameException(int minCharactersNumber, int maxCharactersNumber) 
    : BaseIamCoreException($"Nazwa uprawnienia może posiadać od {minCharactersNumber} do {maxCharactersNumber} znaków oraz musi posiadać prefix w postaci nazwy modułu.", "012")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}