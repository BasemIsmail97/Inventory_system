using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contract;
using Shards.DTOS.AuthDtos;

namespace YourProject.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    [Route("Manager/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: Manager/Auth/Register
        // Manager يقدر يسجل Employee فقط
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Manager/Auth/Register
        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            // Manager يقدر يسجل Employee بس
            if (registerDto.role != "Employee")
            {
                ModelState.AddModelError("", "You can only register Employee accounts");
                return View(registerDto);
            }

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.IsAuthenticated)
            {
                ModelState.AddModelError("", result.Message);
                if (result.Errors != null && result.Errors.Any())
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                return View(registerDto);
            }

            TempData["SuccessMessage"] = "Employee account created successfully!";
            return RedirectToAction("Index", "Product");
        }

        // GET: Manager/Auth/Login
        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Manager/Auth/Login
        [AllowAnonymous]
        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var result = await _authService.LoginAsync(loginDto);

            if (!result.IsAuthenticated)
            {
                ModelState.AddModelError("", result.Message);
                return View(loginDto);
            }

            // التحقق من أن المستخدم Manager
            if (!result.User.Roles.Contains("Manager"))
            {
                ModelState.AddModelError("", "Access denied. Manager role required.");
                return View(loginDto);
            }

            HttpContext.Session.SetString("JWTToken", result.Token);
            HttpContext.Session.SetString("RefreshToken", result.RefreshToken);

            return RedirectToAction("Index", "Product", new { area = "Manager" });
        }

        // POST: Manager/Auth/Logout
        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = HttpContext.Session.GetString("RefreshToken");
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _authService.RevokeTokenAsync(refreshToken);
            }

            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: Manager/Auth/ChangePassword
        [HttpGet("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Manager/Auth/ChangePassword
        [HttpPost("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return View(changePasswordDto);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login");

            var result = await _authService.ChangePasswordAsync(userId, changePasswordDto);

            if (!result.IsAuthenticated)
            {
                ModelState.AddModelError("", result.Message);
                if (result.Errors != null && result.Errors.Any())
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                return View(changePasswordDto);
            }

            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToAction("Index", "Product");
        }
    }
}