using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Core.Entities.Concrete;

namespace Core.Utilities.Security.Jwt
{
    public interface ITokenHelper
    {
        (AccessToken accessToken, RefreshToken refreshToken) CreateToken(User user, List<OperationClaim> operationClaims);
        ClaimsPrincipal GetPrincipalFromToken(string token);

    }
}
