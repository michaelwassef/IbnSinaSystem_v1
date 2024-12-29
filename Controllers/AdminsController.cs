using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IbnSinaSystem.Controllers
{
    [Authorize]
    public class AdminsController : Controller
    {
        private readonly IAdminUsersService _adminUsersService;

        public AdminsController(IAdminUsersService adminUsersService)
        {
            _adminUsersService = adminUsersService;
        }

        public IActionResult AdminDashboard()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        public IActionResult AdminsPage()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        public IActionResult MessagesPage()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadAdminUsers()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var adminUsers = await _adminUsersService.GetAllAdminUsers();
            return Json(adminUsers);
        }

        [HttpGet]
        public async Task<IActionResult> GetAdminUserById(int adminUserId)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var adminUser = await _adminUsersService.GetAdminUserById(adminUserId);

            if (adminUser == null)
                return NotFound(new { ErrorMessage = "Admin user not found" });

            return Json(adminUser);
        }

        [HttpPost]
        public async Task<IActionResult> AddAdminUser([FromForm] IFormCollection formData)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var values = formData["values"];
            var newAdminUser = new AdminUsersModel();
            JsonConvert.PopulateObject(values, newAdminUser);

            if (!string.IsNullOrEmpty(newAdminUser.adminusers_Password))
            {
                newAdminUser.adminusers_Password = PasswordHasher.HashPassword(newAdminUser.adminusers_Password);
            }

            bool addResult = await _adminUsersService.InsertAdminUser(newAdminUser);

            if (addResult)
                return Ok(new { SuccessMessage = "Admin user added successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while adding the admin user" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAdminUser([FromForm] IFormCollection formData)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var adminUserId = Convert.ToInt32(formData["key"]);
            var values = formData["values"];
            var adminUser = await _adminUsersService.GetAdminUserById(adminUserId);

            if (adminUser == null)
                return NotFound(new { ErrorMessage = "Admin user not found" });

            JsonConvert.PopulateObject(values, adminUser);

            if (!string.IsNullOrEmpty(adminUser.adminusers_Password))
            {
                adminUser.adminusers_Password = PasswordHasher.HashPassword(adminUser.adminusers_Password);
            }

            bool updateResult = await _adminUsersService.UpdateAdminUser(adminUser);

            if (updateResult)
                return Ok(new { SuccessMessage = "Admin user updated successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while updating the admin user" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAdminUser([FromForm] IFormCollection formData)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var adminUserId = Convert.ToInt32(formData["key"]);

            bool deleteResult = await _adminUsersService.DeleteAdminUser(adminUserId);

            if (deleteResult)
                return Ok(new { SuccessMessage = "Admin user deleted successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while deleting the admin user" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAdminDashboardCounts()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var counts = new
            {
                StudentsCount = await _adminUsersService.GetTotalStudentsAsync(),
                ProfessorsCount = await _adminUsersService.GetTotalProfessorsAsync(),
                CoursesCount = await _adminUsersService.GetTotalCoursesAsync()
            };

            return Json(counts);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentsCountByCourse()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var data = await _adminUsersService.GetStudentsCountByCourse();
            return Json(data);
        }

    }
}