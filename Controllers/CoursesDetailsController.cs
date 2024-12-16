using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IbnSinaSystem.Controllers
{
    [Authorize]
    public class CoursesDetailsController : Controller
    {
        private readonly ICoursesDetailsService _coursesDetailsService;
        private readonly IStudentsService _studentsService;

        public CoursesDetailsController(ICoursesDetailsService coursesDetailsService, IStudentsService studentsService)
        {
            _coursesDetailsService = coursesDetailsService;
            _studentsService = studentsService;
        }

        public IActionResult AddCoursesPage()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Student") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        public IActionResult ViewStudentGrade()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Student") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }
        
        public IActionResult ViewLectureTable()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Student") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }
        
        public IActionResult ManageCourses()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Professor") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        public IActionResult CoursesDetailsPage()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadCourseDetails()
        {
            var courseDetails = await _coursesDetailsService.GetAllCourseDetails();
            return Json(courseDetails);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseDetailById(int id)
        {
            var courseDetail = await _coursesDetailsService.GetCourseDetailById(id);

            if (courseDetail == null)
                return NotFound(new { ErrorMessage = "Course detail not found" });

            return Json(courseDetail);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourseDetail([FromForm] IFormCollection formData)
        {
            var values = formData["values"];
            var newCourseDetail = new CoursesDetailsModel();
            JsonConvert.PopulateObject(values, newCourseDetail);

            newCourseDetail.coursesdetails_ID = await _coursesDetailsService.GetMaxCoursesDetailsID() + 1;
            bool addResult = await _coursesDetailsService.InsertCourseDetail(newCourseDetail);

            if (addResult)
                return Ok(new { SuccessMessage = "Course detail added successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while adding the course detail" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourseDetail([FromForm] IFormCollection formData)
        {
            var courseDetailId = Convert.ToInt32(formData["key"]);
            var values = formData["values"];
            var courseDetail = await _coursesDetailsService.GetCourseDetailById(courseDetailId);

            if (courseDetail == null)
                return NotFound(new { ErrorMessage = "Course detail not found" });

            JsonConvert.PopulateObject(values, courseDetail);

            bool updateResult = await _coursesDetailsService.UpdateCourseDetail(courseDetail);

            if (updateResult)
                return Ok(new { SuccessMessage = "Course detail updated successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while updating the course detail" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourseDetail([FromForm] IFormCollection formData)
        {
            var courseDetailId = Convert.ToInt32(formData["key"]);

            bool deleteResult = await _coursesDetailsService.DeleteCourseDetail(courseDetailId);

            if (deleteResult)
                return Ok(new { SuccessMessage = "Course detail deleted successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while deleting the course detail" });
        }

        [HttpGet]
        public async Task<IActionResult> LoadStudentsCourses()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var courses = await _coursesDetailsService.GetAllCoursesByStudentID(Convert.ToInt32(userId));
            return Json(courses);
        }

        [HttpGet]
        public async Task<IActionResult> LoadAvailableCourses()
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var student = await _studentsService.GetStudentById(userId);
            var courseDetails = await _coursesDetailsService.GetAllAvailableCourseDetails(userId, student.students_SemesterIN, student.students_departments_ID);
            return Json(courseDetails);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] int courseId)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var isAdded = await _studentsService.IsCourseAddedAsync(userId, courseId);
            if (isAdded)
            {
                return BadRequest(new { ErrorMessage = "Course already added." });
            }

            var hasConflict = await _studentsService.HasScheduleConflictAsync(userId, courseId);
            if (hasConflict)
            {
                return BadRequest(new { ErrorMessage = "Schedule conflict detected! You already have a lecture during the same period on the same day." });
            }

            var result = await _studentsService.AddCourseAsync(userId, courseId);

            if (result)
                return Ok(new { Message = "Course added successfully!" });
            else
                return BadRequest(new { ErrorMessage = "Failed to add course." });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCourse([FromBody] int courseId)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var isAdded = await _studentsService.IsCourseAddedAsync(userId, courseId);
            if (!isAdded)
            {
                return BadRequest(new { ErrorMessage = "Course not found for this student." });
            }

            var result = await _studentsService.RemoveCourseAsync(userId, courseId);

            if (result)
                return Ok(new { Message = "Course removed successfully!" });
            else
                return BadRequest(new { ErrorMessage = "Failed to remove course." });
        }


        [HttpGet]
        public async Task<IActionResult> LoadStudentGrades()
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var grades = await _coursesDetailsService.GetStudentGradesAsync(userId);
            return Json(grades);
        }

        [HttpGet]
        public async Task<IActionResult> LoadLectureGrid()
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                if (userId <= 0)
                {
                    return BadRequest(new { ErrorMessage = "Invalid user ID." });
                }

                var gridData = await _coursesDetailsService.GetStudentLectureGridAsync(userId);
                return Json(gridData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadLectureGrid: {ex.Message}");
                return StatusCode(500, new { ErrorMessage = "An error occurred while loading the lecture grid." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadProfessorCourses()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var courses = await _coursesDetailsService.GetAllCourseDetailsByProfessorsID(Convert.ToInt32(userId));
            return Json(courses);
        }

        [HttpGet]
        public async Task<IActionResult> ViewStudents(int courseId)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Professor"){ return RedirectToAction("AccessDenied", "Access");}
            var course = await _coursesDetailsService.GetCourseDetailById(courseId);

            ViewData["CourseId"] = course.coursesdetails_ID;
            ViewData["CourseName"] = course.courses_Name;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewStudentsData(int courseId)
        {
            try
            {
                var students = await _coursesDetailsService.GetAllStudentsByCourseID(courseId);
                return Json(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = "Failed to load students. Please try again." });
            }
        }
    }
}
