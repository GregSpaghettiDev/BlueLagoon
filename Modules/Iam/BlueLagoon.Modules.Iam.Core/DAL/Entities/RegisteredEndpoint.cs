using BlueLagoon.Modules.Iam.Core.ValueObjects;
using BlueLagoon.Shared.DevTools.Base.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;
using Path = BlueLagoon.Modules.Iam.Core.ValueObjects.Path;

namespace BlueLagoon.Modules.Iam.Core.DAL.Entities;

internal class RegisteredEndpoint : BaseEntity<RegisteredEndpoint>
{
    protected RegisteredEndpoint()
    {
        Activate();
    }

    private RegisteredEndpoint(ModuleName moduleName, HttpMethod httpMethod, Path endpointPath, OperationId operationId, OperationDescription description)
    {
        Id = Guid.NewGuid();
        ModuleName = moduleName;
        HttpMethod = httpMethod;
        Path = endpointPath;
        OperationId = operationId;
        OperationDescription = description;
        Activate();
    }

    public ModuleName ModuleName { get; private set; }

    public HttpMethod HttpMethod { get; private set; }

    public Path Path { get; private set; }

    public OperationId OperationId { get; private set; }

    public OperationDescription OperationDescription { get; private set; }

    public static RegisteredEndpoint Create(ModuleName moduleName, HttpMethod httpMethod, Path endpointPath, OperationId operationId, OperationDescription description)
       => new(moduleName, httpMethod, endpointPath, operationId, description);

    [InverseProperty(nameof(User.RegisteredEndpointCreators))]
    public virtual User Creator { get; private set; }

    [InverseProperty(nameof(User.RegisteredEndpointModificators))]
    public virtual User Modificator { get; private set; }
}
