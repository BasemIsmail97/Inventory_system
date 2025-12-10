using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contract;
using Shards.DTOS.AuthDtos;

namespace YourProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("Admin/[controller]")]
    public class AuthController : Controller
    {
        private readonly IServiceManger _serviceManger;

        public AuthController(IServiceManger serviceManger)
        {
            _serviceManger = serviceManger;
        }

        // GET: Admin/Auth/Register
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Admin/Auth/Register
        // Admin يقدر يسجل أي Role (Admin, Manager, Employee)
        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            // التحقق من أن الـ Role صحيح
            var allowedRoles = new[] { "Admin", "Manager", "Employee" };
            if (!allowedRoles.Contains(registerDto.role))
            {
                ModelState.AddModelError("", "Invalid role selected");
                return View(registerDto);
            }

            var result = await _serviceManger.AuthService.RegisterAsync(registerDto);

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

            TempData["SuccessMessage"] = "Account created successfully!";
            return RedirectToAction("Index", "Dashboard");
        }

        // GET: Admin/Auth/Login
        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Admin/Auth/Login
        [AllowAnonymous]
        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var result = await _serviceManger.AuthService.LoginAsync(loginDto);

            if (!result.IsAuthenticated)
            {
                ModelState.AddModelError("", result.Message);
                return View(loginDto);
            }

            // حفظ الـ Token في الـ Session أو Cookie
            HttpContext.Session.SetString("JWTToken", result.Token);
            HttpContext.Session.SetString("RefreshToken", result.RefreshToken);

            // التوجيه حسب الـ Role
            if (result.User.Roles.Contains("Admin"))
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            else if (result.User.Roles.Contains("Manager"))
                return RedirectToAction("Index", "Product", new { area = "Manager" });
            else
                return RedirectToAction("Index", "Product", new { area = "Employee" });
        }

        // POST: Admin/Auth/Logout
        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = HttpContext.Session.GetString("RefreshToken");
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _serviceManger.AuthService.RevokeTokenAsync(refreshToken);
            }

            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: Admin/Auth/ChangePassword
        [HttpGet("ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Admin/Auth/ChangePassword
        [HttpPost("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return View(changePasswordDto);

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login");

            var result = await _serviceManger.AuthService.ChangePasswordAsync(userId, changePasswordDto);

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
            return RedirectToAction("Index", "Dashboard");
        }
    }
}