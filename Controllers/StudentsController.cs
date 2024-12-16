using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IbnSinaSystem.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly IStudentsService _studentsService;

        public StudentsController(IStudentsService studentsService)
        {
            _studentsService = studentsService;
        }

        [HttpGet]
        public IActionResult StudentDashboard()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Student") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public IActionResult EditStudentProfile()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Student") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public IActionResult StudentsPage()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadStudents()
        {
            var students = await _studentsService.GetAllStudents();
            return Json(students);
        }
        
        [HttpGet]
        public async Task<IActionResult> LoadStudentprofile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var students = await _studentsService.GetStudentById(Convert.ToInt32(userId));
            return Json(students);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            var student = await _studentsService.GetStudentById(studentId);

            if (student == null)
                return NotFound(new { ErrorMessage = "Student not found" });

            return Json(student);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromForm] IFormCollection formData)
        {
            var values = formData["values"];
            var newStudent = new StudentsModel();
            JsonConvert.PopulateObject(values, newStudent);

            newStudent.students_Password = PasswordHasher.HashPassword(newStudent.students_Password);
            newStudent.students_ID = await _studentsService.GetMaxStudentsID() + 1;

            bool addResult = await _studentsService.InsertStudent(newStudent);

            if (addResult)
                return Ok(new { SuccessMessage = "Student added successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while adding the student" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent([FromForm] IFormCollection formData)
        {
            var studentId = Convert.ToInt32(formData["key"]);
            var values = formData["values"];
            var student = await _studentsService.GetStudentById(studentId);

            if (student == null)
                return NotFound(new { ErrorMessage = "Student not found" });

            JsonConvert.PopulateObject(values, student);

            // Hash the password if it has been updated
            if (!string.IsNullOrEmpty(student.students_Password))
            {
                student.students_Password = PasswordHasher.HashPassword(student.students_Password);
            }

            bool updateResult = await _studentsService.UpdateStudent(student);

            if (updateResult)
                return Ok(new { SuccessMessage = "Student updated successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while updating the student" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] StudentsModel student)
        {
            var userId =Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            if(userId != student.students_ID)
                return BadRequest(new { ErrorMessage = "You cannot edit profile data other than your own." });

            if (student == null)
                return BadRequest(new { ErrorMessage = "Invalid data" });

            var existingStudent = await _studentsService.GetStudentById(student.students_ID);

            if (existingStudent == null)
                return NotFound(new { ErrorMessage = "Student not found" });

            // Update fields
            existingStudent.students_Name = student.students_Name;
            existingStudent.students_PhoneNumber = student.students_PhoneNumber;
            existingStudent.students_BirthDay = student.students_BirthDay;

            // Hash the password if it has been updated
            if (!string.IsNullOrEmpty(student.students_Password))
            {
                existingStudent.students_Password = PasswordHasher.HashPassword(student.students_Password);
            }

            bool updateResult = await _studentsService.UpdateStudent(existingStudent);

            if (updateResult)
                return Ok(new { SuccessMessage = "Student updated successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while updating the student" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStudent([FromForm] IFormCollection formData)
        {
            var studentId = Convert.ToInt32(formData["key"]);

            bool deleteResult = await _studentsService.DeleteStudent(studentId);

            if (deleteResult)
                return Ok(new { SuccessMessage = "Student deleted successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while deleting the student" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            try
            {
                var messages = await _studentsService.GetAllMessagesAsync();
                return Json(messages);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllMessages: {ex.Message}");
                return StatusCode(500, new { ErrorMessage = "Failed to retrieve messages. Please try again." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContactForm([FromForm] ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { ErrorMessage = "Please fill out all required fields correctly." });
            }

            try
            {
                var success = await _studentsService.SaveContactFormAsync(model);

                if (success)
                {
                    return Ok(new { Message = "Thank you for contacting us! We will get back to you soon." });
                }
                else
                {
                    return StatusCode(500, new { ErrorMessage = "Failed to save your message. Please try again." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = "An error occurred while processing your request." });
            }
        }
    }
}