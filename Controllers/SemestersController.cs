using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IbnSinaSystem.Controllers
{
    [Authorize]
    public class SemestersController : Controller
    {
        private readonly ISemestersService _semestersService;

        public SemestersController(ISemestersService semestersService)
        {
            _semestersService = semestersService;
        }

        public IActionResult SemestersPage()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadSemesters()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var semesters = await _semestersService.GetAllSemesters();
            return Json(semesters);
        }

        [HttpPost]
        public async Task<IActionResult> AddSemester([FromForm] IFormCollection formData)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var values = formData["values"];
            var newSemester = new SemestersModel();
            JsonConvert.PopulateObject(values, newSemester);

            bool addResult = await _semestersService.InsertSemester(newSemester);

            if (addResult)
                return Ok(new { SuccessMessage = "Semester added successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while adding the semester" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSemester([FromForm] IFormCollection formData)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var semesterId = Convert.ToInt32(formData["key"]);
            var values = formData["values"];
            var semester = await _semestersService.SelectSemesterByID(semesterId);

            if (semester == null)
                return NotFound(new { ErrorMessage = "Semester not found" });

            JsonConvert.PopulateObject(values, semester);

            bool updateResult = await _semestersService.UpdateSemester(semester);

            if (updateResult)
                return Ok(new { SuccessMessage = "Semester updated successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while updating the semester" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSemester([FromForm] IFormCollection formData)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var semesterId = Convert.ToInt32(formData["key"]);

            bool deleteResult = await _semestersService.DeleteSemester(semesterId);

            if (deleteResult)
                return Ok(new { SuccessMessage = "Semester deleted successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while deleting the semester" });
        }
    }
}
