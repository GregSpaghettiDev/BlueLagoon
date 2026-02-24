using BlueLagoon.Shared.Infrastructure.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal class EndpointNotFoundException(Guid id) : BaseCoreException($"Endpoint o podanym id {id} nie został odnaleziony.", "023")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}