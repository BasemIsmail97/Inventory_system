using Shards.DTOS.AuthDtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contract
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> CreateJwtTokenAsync(UserInfoDto user);
        RefreshTokenDto GenerateRefreshToken();
        string GenerateRandomToken(int length = 32);
    }
}
