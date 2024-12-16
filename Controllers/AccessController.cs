using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IbnSinaSystem.Controllers
{
    public class AccessController : Controller
    {
        private readonly IAccessService _accessService;
        private readonly IDepartmentsService _departmentsService;
        private readonly IStudentsService _studentsService;

        public AccessController(IAccessService accessService, IDepartmentsService departmentsService, IStudentsService studentsService)
        {
            _accessService = accessService;
            _departmentsService = departmentsService;
            _studentsService = studentsService;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
            {
                var roleClaim = claimUser.FindFirst("Role")?.Value;
                if (!string.IsNullOrEmpty(roleClaim))
                {
                    switch (roleClaim)
                    {
                        case "Student":
                            return RedirectToAction("StudentDashboard", "Students");
                        case "Admin":
                            return RedirectToAction("AdminDashboard", "Admins");
                        case "Professor":
                            return RedirectToAction("ProfessorDashboard", "Professors");
                        default:
                            return RedirectToAction("Index", "Home");
                    }
                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginsModel modelLogin)
        {
            try
            {
                var authenticationResult = await _accessService.AuthenticateUserAsync(modelLogin);

                if (authenticationResult.IsAuthenticated)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, authenticationResult.userid.ToString()),
                        new Claim(ClaimTypes.Role, authenticationResult.Role),
                        new Claim("exp", ((DateTimeOffset)DateTime.UtcNow.AddHours(4)).ToUnixTimeSeconds().ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authenticationProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = modelLogin.KeepLoggedIn
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProperties);

                    switch (authenticationResult.Role)
                    {
                        case "Student":
                            return RedirectToAction("StudentDashboard", "Students");
                        case "Admin":
                            return RedirectToAction("AdminDashboard", "Admins");
                        case "Professor":
                            return RedirectToAction("ProfessorDashboard", "Professors");
                        default:
                            TempData["ErrorMessage"] = "Invalid role assigned to the user.";
                            return View(modelLogin);
                    }
                }
                else
                {
                    TempData["ValidateMessage"] = "Invalid UserName or Password";
                    return View(modelLogin);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing the login request: " + ex.Message;
                return View(modelLogin);
            }
        }

        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            var departments = await _departmentsService.GetAllDepartments();
            ViewBag.Departments = departments;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] StudentsModel newStudent)
        {
            try
            {
                newStudent.students_Password = PasswordHasher.HashPassword(newStudent.students_Password);
                newStudent.students_SemesterIN = 1;
                newStudent.students_GPA = 0;
                newStudent.students_ID = await _studentsService.GetMaxStudentsID() + 1;

                bool addResult = await _studentsService.InsertStudent(newStudent);

                if (addResult)
                    return Ok(new { SuccessMessage = "Student signed up successfully", success = true });
                else
                    return BadRequest(new { ErrorMessage = "Error occurred while signing up the student", success = false });
            }
            catch
            {
                return StatusCode(500, new { ErrorMessage = "Internal server error", success = false });
            }
        }

    }
}