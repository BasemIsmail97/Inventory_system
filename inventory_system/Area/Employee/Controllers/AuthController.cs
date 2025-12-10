using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contract;
using Shards.DTOS.AuthDtos;

namespace inventory_system.Area.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    [Route("Employee/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: Employee/Auth/Login
        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Employee/Auth/Login
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

            // التحقق من أن المستخدم Employee
            if (!result.User.Roles.Contains("Employee"))
            {
                ModelState.AddModelError("", "Access denied. Employee role required.");
                return View(loginDto);
            }

            HttpContext.Session.SetString("JWTToken", result.Token);
            HttpContext.Session.SetString("RefreshToken", result.RefreshToken);

            return RedirectToAction("Index", "Product", new { area = "Employee" });
        }
        // POST: Employee/Auth/Logout
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

    }
}
