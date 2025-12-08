
using AutoMapper;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.Abstraction.Contract;
using Shards.DTOS.AuthDtos;
using Shards.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            IMapper mapper,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

        #region Register
        public async Task<AuthResultDto> RegisterAsync(RegisterDto registerDto)
        {
            // Check if email already exists
            if (await _userManager.FindByEmailAsync(registerDto.Email) is not null)
            {
                return new AuthResultDto
                {
                    IsAuthenticated = false,
                    Message = "The email address is already in use"
                };
            }

            // Check if username already exists
            if (await _userManager.FindByNameAsync(registerDto.UserName) is not null)
            {
                return new AuthResultDto
                {
                    IsAuthenticated = false,
                    Message = "The username is already in use"
                };
            }

            // Create new user
            var user = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
                EmailConfirmed = true,
                
                
                
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return new AuthResultDto
                {
                    IsAuthenticated = false,
                    Message = "Account creation failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            // Assign default role (Employee)
            await _userManager.AddToRoleAsync(user, registerDto.role);


            // Generate JWT Token
            var UserInfoDto = _mapper.Map<UserInfoDto>(user);
            var jwtToken = await _tokenService.CreateJwtTokenAsync(UserInfoDto);
            var refreshToken = _tokenService.GenerateRefreshToken();

            UserInfoDto.RefreshTokens.Add(refreshToken);
            var UserAfterAddingTokens= _mapper.Map<ApplicationUser>(UserInfoDto);
            await _userManager.UpdateAsync(UserAfterAddingTokens);

            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResultDto
            {
                IsAuthenticated = true,
                Message = "Account created successfully",
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = refreshToken.Token,
                ExpiresOn = jwtToken.ValidTo,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                }
            };
        }
        #endregion

        #region Login
        public async Task<AuthResultDto> LoginAsync(LoginDto loginDto)
        {
            var authResult = new AuthResultDto();

            // Find user by email
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                authResult.Message = "Incorrect email address or password";
                return authResult;
            }


            // Check if account is locked
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                authResult.Message = "The account is temporarily suspended.";
                return authResult;
            }

            // Generate JWT Token
            var userInfoDto = _mapper.Map<UserInfoDto>(user);
            var jwtToken = await _tokenService.CreateJwtTokenAsync(userInfoDto);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var roles = await _userManager.GetRolesAsync(user);

            // Revoke old refresh tokens (keep only last 5)
            if (user.RefreshTokens.Any())
            {
                var activeTokens = user.RefreshTokens
                    .OrderByDescending(t => t.CreatedOn)
                    .Take(5)
                    .ToList();

                user.RefreshTokens = activeTokens;
            }

            userInfoDto.RefreshTokens.Add(refreshToken);
            var userAfterAddingTokens = _mapper.Map<ApplicationUser>(userInfoDto);
            await _userManager.UpdateAsync(userAfterAddingTokens);

            authResult.IsAuthenticated = true;
            authResult.Message = "Login successful";
            authResult.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authResult.RefreshToken = refreshToken.Token;
            authResult.ExpiresOn = jwtToken.ValidTo;
            authResult.User = new UserInfoDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList()
            };

            return authResult;
        }
        #endregion

        #region Refresh Token
        public async Task<AuthResultDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var authResult = new AuthResultDto();

            // Find user by refresh token
            var user = await _userManager.Users
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshTokenDto.RefreshToken));

            if (user is null)
            {
                authResult.Message = "Invalid Token";
                return authResult;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == refreshTokenDto.RefreshToken);

            if (!refreshToken.IsActive)
            {
                authResult.Message = "The Token is inactive.";
                return authResult;
            }

            // Revoke old refresh token
            refreshToken.RevokedOn = DateTime.Now;

            // Generate new tokens
            var UserInfoDto = _mapper.Map<UserInfoDto>(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            UserInfoDto.RefreshTokens.Add(newRefreshToken);
            var userAfterAddingTokens = _mapper.Map<ApplicationUser>(UserInfoDto);
            await _userManager.UpdateAsync(userAfterAddingTokens);

            var jwtToken = await _tokenService.CreateJwtTokenAsync(UserInfoDto);
            var roles = await _userManager.GetRolesAsync(user);

            authResult.IsAuthenticated = true;
            authResult.Message = "The Token was successfully renewed.";
            authResult.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authResult.RefreshToken = newRefreshToken.Token;
            authResult.ExpiresOn = jwtToken.ValidTo;
            authResult.User = new UserInfoDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList()
            };

            return authResult;
        }
        #endregion

        #region Revoke Token (Logout)
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user is null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.Now;
            await _userManager.UpdateAsync(user);

            return true;
        }
        #endregion

        #region Change Password
        public async Task<AuthResultDto> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
        {
            var authResult = new AuthResultDto();

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                authResult.Message = "User not found";
                return authResult;
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                changePasswordDto.CurrentPassword,
                changePasswordDto.NewPassword
            );

            if (!result.Succeeded)
            {
                authResult.Message = "Password change failed";
                authResult.Errors = result.Errors.Select(e => e.Description).ToList();
                return authResult;
            }

            authResult.IsAuthenticated = true;
            authResult.Message = "The password has been successfully changed.";

            return authResult;
        }
        #endregion


        #region Reset Password
        public async Task<AuthResultDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var authResult = new AuthResultDto();

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

            if (user is null)
            {
                authResult.Message = "User not found";
                return authResult;
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                resetPasswordDto.Token,
                resetPasswordDto.NewPassword
            );

            if (!result.Succeeded)
            {
                authResult.Message = "Password reset failed";
                authResult.Errors = result.Errors.Select(e => e.Description).ToList();
                return authResult;
            }

            authResult.IsAuthenticated = true;
            authResult.Message = "The password was successfully reset.";

            return authResult;
        }
        #endregion

        

       
    }
}