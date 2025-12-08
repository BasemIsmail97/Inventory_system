using Shards.DTOS.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contract
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
        Task<AuthResultDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        Task<bool> RevokeTokenAsync(string token);
        Task<AuthResultDto> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
        
        Task<AuthResultDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
       
       
    }
}
