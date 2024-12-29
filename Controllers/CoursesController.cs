using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IbnSinaSystem.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICoursesService _coursesService;

        public CoursesController(ICoursesService coursesService)
        {
            _coursesService = coursesService;
        }

        public IActionResult CoursesPage()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadCourses()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var courses = await _coursesService.GetAllCourses();
            return Json(courses);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromForm] IFormCollection formData)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var values = formData["values"];
            var newCourse = new CoursesModel();
            JsonConvert.PopulateObject(values, newCourse);

            bool addResult = await _coursesService.Insertcourse(newCourse);

            if (addResult)
                return Ok(new { SuccessMessage = "Course added successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while adding the course" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse([FromForm] IFormCollection formData)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var courseId = Convert.ToInt32(formData["key"]);
            var values = formData["values"];
            var course = await _coursesService.SelectcourseByID(courseId);

            if (course == null)
                return NotFound(new { ErrorMessage = "Course not found" });

            JsonConvert.PopulateObject(values, course);

            bool updateResult = await _coursesService.Updatecourse(course);

            if (updateResult)
                return Ok(new { SuccessMessage = "Course updated successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while updating the course" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse([FromForm] IFormCollection formData)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return BadRequest("You Don't Have Permission To Access"); }

            var courseId = Convert.ToInt32(formData["key"]);

            bool deleteResult = await _coursesService.Deletecourse(courseId);

            if (deleteResult)
                return Ok(new { SuccessMessage = "Course deleted successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while deleting the course" });
        }
    }
}