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
    public class ProfessorsController : Controller
    {
        private readonly IProfessorsService _professorsService;

        public ProfessorsController(IProfessorsService professorsService)
        {
            _professorsService = professorsService;
        }

        [HttpGet]
        public IActionResult ProfessorDashboard()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Professor") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public IActionResult EditProfessorProfile()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Professor") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public IActionResult ProfessorsPage()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

       
        [HttpGet]
        public async Task<IActionResult> LoadProfessors()
        {
            var professors = await _professorsService.GetAllProfessors();
            return Json(professors);
        }

        [HttpGet]
        public async Task<IActionResult> LoadProfessorprofile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var professor = await _professorsService.GetProfessorById(Convert.ToInt32(userId));
            return Json(professor);
        }

        [HttpGet]
        public async Task<IActionResult> GetProfessorById(int professorId)
        {
            var professor = await _professorsService.GetProfessorById(professorId);

            if (professor == null)
                return NotFound(new { ErrorMessage = "Professor not found" });

            return Json(professor);
        }

        [HttpPost]
        public async Task<IActionResult> AddProfessor([FromForm] IFormCollection formData)
        {
            var values = formData["values"];
            var newProfessor = new ProfessorsModel();
            JsonConvert.PopulateObject(values, newProfessor);

            // Hash the password before saving
            if (!string.IsNullOrEmpty(newProfessor.professors_Password))
            {
                newProfessor.professors_Password = PasswordHasher.HashPassword(newProfessor.professors_Password);
            }

            newProfessor.professors_ID = await _professorsService.GetMaxprofessorsID()+1;
            bool addResult = await _professorsService.InsertProfessor(newProfessor);

            if (addResult)
                return Ok(new { SuccessMessage = "Professor added successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while adding the professor" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfessor([FromForm] IFormCollection formData)
        {
            var professorId = Convert.ToInt32(formData["key"]);
            var values = formData["values"];
            var professor = await _professorsService.GetProfessorById(professorId);

            if (professor == null)
                return NotFound(new { ErrorMessage = "Professor not found" });

            JsonConvert.PopulateObject(values, professor);

            // Hash the password if it has been updated
            if (!string.IsNullOrEmpty(professor.professors_Password))
            {
                professor.professors_Password = PasswordHasher.HashPassword(professor.professors_Password);
            }

            bool updateResult = await _professorsService.UpdateProfessor(professor);

            if (updateResult)
                return Ok(new { SuccessMessage = "Professor updated successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while updating the professor" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfessorsModel prof)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            if (userId != prof.professors_ID)
                return BadRequest(new { ErrorMessage = "You cannot edit profile data other than your own." });

            if (prof == null)
                return BadRequest(new { ErrorMessage = "Invalid data" });

            var existingProf = await _professorsService.GetProfessorById(prof.professors_ID);

            if (existingProf == null)
                return NotFound(new { ErrorMessage = "Professor not found" });

            // Update fields
            existingProf.professors_Name = prof.professors_Name;
            existingProf.professors_PhoneNumber = prof.professors_PhoneNumber;

            // Hash the password if it has been updated
            if (!string.IsNullOrEmpty(prof.professors_Password))
            {
                existingProf.professors_Password = PasswordHasher.HashPassword(prof.professors_Password);
            }

            bool updateResult = await _professorsService.UpdateProfessor(existingProf);

            if (updateResult)
                return Ok(new { SuccessMessage = "Professor updated successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while updating the professor" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProfessor([FromForm] IFormCollection formData)
        {
            var professorId = Convert.ToInt32(formData["key"]);

            bool deleteResult = await _professorsService.DeleteProfessor(professorId);

            if (deleteResult)
                return Ok(new { SuccessMessage = "Professor deleted successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while deleting the professor" });
        }
    }
}