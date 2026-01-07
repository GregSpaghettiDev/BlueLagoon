using Common.BaseValueObjects;
using Common.Date;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Common.JwtExtensions;

public static class ClaimsPrincipalExtensions
{
    public static BaseDate GetUtcExpirationDate(this ClaimsPrincipal principal)
    {
        var unixTimestampExpirationDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

        return new BaseDate(unixTimestampExpirationDate.UnixTimeStampToDateTime());
    }

    public static Guid GetClaimValueAsGuid(this ClaimsPrincipal principal, string claimTypeName)
    {
        var value = principal.Claims.SingleOrDefault(x => x.Type == claimTypeName);
        if (Guid.TryParse(value?.Value, out Guid guid))
            return guid;
        return Guid.Empty;
    }
}
