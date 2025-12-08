using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shards.DTOS.AuthDtos;
using Shards.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public TokenService(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings ,IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

        #region Create JWT Token
        public async Task<JwtSecurityToken> CreateJwtTokenAsync(UserInfoDto _user)
        {
            var appUser = await _userManager.FindByIdAsync(_user.Id);
            if (appUser == null)
            {
                throw new Exception("User not found");
            }
            var  user = _mapper.Map<ApplicationUser>(appUser);
            // Get user claims
            var userClaims = await _userManager.GetClaimsAsync(user);

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

            // Combine all claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("FullName", $"{user.FirstName} {user.LastName}")
            }
            .Union(userClaims)
            .Union(roleClaims);

            // Create symmetric security key
            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
            );

            // Create signing credentials
            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey,
                SecurityAlgorithms.HmacSha256
            );

            // Create JWT token
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }
        #endregion

        #region Generate Refresh Token
        public RefreshTokenDto GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            
            
            return _mapper.Map<RefreshTokenDto>( new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.Now.AddDays(_jwtSettings.RefreshTokenDurationInDays),
                CreatedOn = DateTime.Now
            });

        }
        #endregion

        #region Generate Random Token (for Email Verification, Password Reset)
        public string GenerateRandomToken(int length = 32)
        {
            var randomNumber = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        #endregion
    }
}
