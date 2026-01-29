using BlueLagoon.Shared.DevTools.Base.Abstractions;
using BlueLagoon.Shared.DevTools.Base.Exceptions;
using OpenIddict.EntityFrameworkCore.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueLagoon.Modules.Iam.Core.DAL.Entities;

internal class Module : OpenIddictEntityFrameworkCoreScope<Guid>, IBaseEntity
{
    public Module()
    {
        Activate();
    }

    public DateTime CreatedAt { get; private set; }

    public Guid CreatorId { get; private set; }

    public DateTime? ModifiedAt { get; private set; }

    public Guid? ModificatorId { get; private set; }

    public bool IsActive { get; private set; }

    public string BaseUrl { get; private set; }

    public string OpenApiPath { get; private set; }

    public void SetBaseUrl(string baseUrl)
    {
        BaseUrl = baseUrl;
    }

    public void SetOpenApiPath(string openApiPath)
    {
        OpenApiPath = openApiPath;
    }

    public void SetCreatorAuditProperties(DateTime createdAt, Guid creatorId)
    {
        CreatedAt = createdAt;
        CreatorId = creatorId;
    }

    public void SetModificatorAuditProperties(DateTime modifiedAt, Guid modificatorId)
    {
        ModifiedAt = modifiedAt;
        ModificatorId = modificatorId;
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidActivationFlagException(nameof(Module));

        if (!IsActive)
            IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidDeactivationFlagException(nameof(Module));

        if (IsActive)
            IsActive = false;
    }

    [InverseProperty(nameof(User.ModuleCreators))]
    public virtual User Creator { get; private set; }

    [InverseProperty(nameof(User.ModuleModificators))]
    public virtual User Modificator { get; private set; }
}