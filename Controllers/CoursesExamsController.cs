using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using IbnSinaSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IbnSinaSystem.Controllers
{
    [Authorize]
    public class CoursesExamsController : Controller
    {
        private readonly ICoursesExamsDetailsService _examsService;
        private readonly ICoursesDetailsService _coursesDetailsService;

        public CoursesExamsController(ICoursesExamsDetailsService examsService, ICoursesDetailsService coursesDetailsService)
        {
            _examsService = examsService;
            _coursesDetailsService = coursesDetailsService;
        }

        public IActionResult ManageExamsPage()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Professor") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadExamsByCourse(int courseId)
        {
            var exams = await _examsService.GetAllCoursesExamsDetailsByCourse(courseId);
            return Json(exams);
        }

        [HttpPost]
        public async Task<IActionResult> AddExam([FromBody] CoursesExamsDetailsModel newExam)
        {
            if (newExam == null)
            {
                return BadRequest(new { ErrorMessage = "Invalid data sent" });
            }

            try
            {
                var currentTotalMarks = await _examsService.GetTotalMarksByCourse(newExam.coursesexamsdetails_coursesdetails_ID);
                if (currentTotalMarks + newExam.coursesexamsdetails_TotalMarks > 100)
                {
                    return BadRequest(new { ErrorMessage = "Adding this exam exceeds the total marks limit of 100 for the course." });
                }

                bool addResult = await _examsService.InsertCoursesExamsDetails(newExam);

                if (addResult)
                    return Ok(new { SuccessMessage = "Exam added successfully" });
                else
                    return BadRequest(new { ErrorMessage = "Error occurred while adding the exam" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = "An unexpected error occurred." });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExam([FromBody] CoursesExamsDetailsModel updatedExam)
        {
            if (updatedExam == null || updatedExam.coursesexamsdetails_ID <= 0)
            {
                return BadRequest(new { ErrorMessage = "Invalid exam data or missing ID." });
            }

            try
            {
                var existingExam = await _examsService.GetCoursesExamsDetailsById(updatedExam.coursesexamsdetails_ID);

                if (existingExam == null)
                    return NotFound(new { ErrorMessage = "Exam not found" });

                var currentTotalMarks = await _examsService.GetTotalMarksByCourse(existingExam.coursesexamsdetails_coursesdetails_ID);
                var newTotalMarks = currentTotalMarks - existingExam.coursesexamsdetails_TotalMarks + updatedExam.coursesexamsdetails_TotalMarks;

                if (newTotalMarks > 100){return BadRequest(new { ErrorMessage = "Updating this exam exceeds the total marks limit of 100 for the course." });}

                existingExam.coursesexamsdetails_Name = updatedExam.coursesexamsdetails_Name ?? existingExam.coursesexamsdetails_Name;
                existingExam.coursesexamsdetails_Date = updatedExam.coursesexamsdetails_Date != DateTime.MinValue
                    ? updatedExam.coursesexamsdetails_Date
                    : existingExam.coursesexamsdetails_Date;
                existingExam.coursesexamsdetails_TotalMarks = updatedExam.coursesexamsdetails_TotalMarks > 0
                    ? updatedExam.coursesexamsdetails_TotalMarks
                    : existingExam.coursesexamsdetails_TotalMarks;

                bool updateResult = await _examsService.UpdateCoursesExamsDetails(existingExam);

                if (updateResult)
                    return Ok(new { SuccessMessage = "Exam updated successfully" });
                else
                    return BadRequest(new { ErrorMessage = "Error occurred while updating the exam" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = "An unexpected error occurred." });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExam([FromBody] CoursesExamsDetailsModel model)
        {
            if (model.coursesexamsdetails_ID <= 0)
            {
                return BadRequest(new { ErrorMessage = "Invalid exam ID provided." });
            }

            await _examsService.DeleteCoursesExamsMarksByExam(model.coursesexamsdetails_ID);
            bool deleteResult = await _examsService.DeleteCoursesExamsDetails(model.coursesexamsdetails_ID);

            if (deleteResult)
                return Ok(new { SuccessMessage = "Exam deleted successfully." });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while deleting the exam." });
        }


        [HttpGet]
        public async Task<IActionResult> AddMarksForStudents(int examId)
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Professor") { return RedirectToAction("AccessDenied", "Access"); }
            var exam = await _examsService.GetCoursesExamsDetailsById(examId);

            ViewData["CourseId"] = exam.coursesexamsdetails_coursesdetails_ID;
            ViewData["Id"] = exam.coursesexamsdetails_ID;
            ViewData["Name"] = exam.coursesexamsdetails_Name;
            ViewData["Total"] = exam.coursesexamsdetails_TotalMarks;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewStudentsData(int courseId)
        {
            try
            {
                var students = await _coursesDetailsService.GetAllMarksStudentsByCourseID(courseId);
                return Json(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = "Failed to load students. Please try again." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveExamGrades([FromBody] List<CoursesExamsMarksModel> grades)
        {
            if (grades == null || grades.Count == 0) { return BadRequest(new { success = false, message = "No grades provided." }); }

            var errors = new List<string>();
            var validGrades = new List<CoursesExamsMarksModel>();

            try
            {
                foreach (var grade in grades)
                {
                    var examDetails = await _examsService.GetCoursesExamsDetailsById(grade.coursesexamsmarks_coursesexamsdetails_ID);

                    if (grade.coursesexamsdetails_MarkOfStudent > examDetails.coursesexamsdetails_TotalMarks)
                    {
                        errors.Add($"{grade.coursesexamsdetails_students_ID}.");
                        continue;
                    }

                    validGrades.Add(grade);
                }

                foreach (var grade in validGrades){ await _examsService.AddOrUpdateGradeAsync(grade);}

                if (errors.Any())
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Some grades were not saved due to validation errors.",
                    });
                }
                return Ok(new { success = true, message = "All grades saved successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while saving grades.", details = ex.Message });
            }
        }
    }
}
