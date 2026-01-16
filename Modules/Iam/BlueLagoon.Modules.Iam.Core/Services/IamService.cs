using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueLagoon.Modules.Iam.Core.Services;

internal sealed class IamService(IOpenIddictScopeManager scopeManager) : IIamService
{
    public async Task<IEnumerable<ModuleDto>> GetModulesAsync()
    {
        return null;
    }
}
