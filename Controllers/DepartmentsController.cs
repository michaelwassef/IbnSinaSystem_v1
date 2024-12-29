using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IbnSinaSystem.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentsService _departmentsService;

        public DepartmentsController(IDepartmentsService departmentsService)
        {
            _departmentsService = departmentsService;
        }

        public IActionResult DepartmentsPage()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadDepartments()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var departments = await _departmentsService.GetAllDepartments();
            return Json(departments);
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromForm] IFormCollection formData)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var values = formData["values"];
            var newDepartment = new DepartmentsModel();
            JsonConvert.PopulateObject(values, newDepartment);

            bool addResult = await _departmentsService.Insertdepartment(newDepartment);

            if (addResult)
                return Ok(new { SuccessMessage = "Department added successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while adding the department" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDepartment([FromForm] IFormCollection formData)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var departmentId = Convert.ToInt32(formData["key"]);
            var values = formData["values"];
            var department = await _departmentsService.SelectdepartmentByID(departmentId);

            if (department == null)
                return NotFound(new { ErrorMessage = "Department not found" });

            JsonConvert.PopulateObject(values, department);

            bool updateResult = await _departmentsService.Updatedepartment(department);

            if (updateResult)
                return Ok(new { SuccessMessage = "Department updated successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while updating the department" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDepartment([FromForm] IFormCollection formData)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var departmentId = Convert.ToInt32(formData["key"]);

            bool deleteResult = await _departmentsService.Deletedepartment(departmentId);

            if (deleteResult)
                return Ok(new { SuccessMessage = "Department deleted successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while deleting the department" });
        }
    }
}