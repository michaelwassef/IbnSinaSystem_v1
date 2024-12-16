using IbnSinaSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace IbnSinaSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!string.IsNullOrEmpty(userRole))
            {
                switch (userRole)
                {
                    case "Student":
                        return RedirectToAction("StudentDashboard", "Students");
                    case "Admin":
                        return RedirectToAction("AdminDashboard", "Admins");
                    case "Professor":
                        return RedirectToAction("ProfessorDashboard", "Professors");
                    default:
                        return RedirectToAction("AccessDenied", "Access");
                }
            }
            return RedirectToAction("Login", "Access");
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}