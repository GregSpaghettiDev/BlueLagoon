using BlueLagoon.Shared.Infrastructure.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal sealed class UserNotFoundException(Guid id) : BaseCoreException($"Użytkownik o podanym id {id} nie został odnaleziony.", "022")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}
