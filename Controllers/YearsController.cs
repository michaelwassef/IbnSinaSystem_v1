using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IbnSinaSystem.Controllers
{
    [Authorize]
    public class YearsController : Controller
    {
        private readonly IYearsService _yearsService;

        public YearsController(IYearsService yearsService)
        {
            _yearsService = yearsService;
        }

        public IActionResult YearsPage()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadYears()
        {
            var years = await _yearsService.GetAllYears();
            return Json(years);
        }

        [HttpPost]
        public async Task<IActionResult> AddYear([FromForm] IFormCollection formData)
        {
            var values = formData["values"];
            var newYear = new YearsModel();
            JsonConvert.PopulateObject(values, newYear);

            bool addResult = await _yearsService.InsertYear(newYear);

            if (addResult)
                return Ok(new { SuccessMessage = "Year added successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while adding the year" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateYear([FromForm] IFormCollection formData)
        {
            var yearId = Convert.ToInt32(formData["key"]);
            var values = formData["values"];
            var year = await _yearsService.SelectYearByID(yearId);

            if (year == null)
                return NotFound(new { ErrorMessage = "Year not found" });

            JsonConvert.PopulateObject(values, year);

            bool updateResult = await _yearsService.UpdateYear(year);

            if (updateResult)
                return Ok(new { SuccessMessage = "Year updated successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while updating the year" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteYear([FromForm] IFormCollection formData)
        {
            var yearId = Convert.ToInt32(formData["key"]);

            bool deleteResult = await _yearsService.DeleteYear(yearId);

            if (deleteResult)
                return Ok(new { SuccessMessage = "Year deleted successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while deleting the year" });
        }
    }
}