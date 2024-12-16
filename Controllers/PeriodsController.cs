using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IbnSinaSystem.Controllers
{
    [Authorize]
    public class PeriodsController : Controller
    {
        private readonly IPeriodsService _periodsService;

        public PeriodsController(IPeriodsService periodsService)
        {
            _periodsService = periodsService;
        }

        public IActionResult PeriodsPage()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadPeriods()
        {
            var periods = await _periodsService.GetAllPeriods();
            return Json(periods);
        }

        [HttpPost]
        public async Task<IActionResult> AddPeriod([FromForm] IFormCollection formData)
        {
            var values = formData["values"];
            var newPeriod = new PeriodsModel();
            JsonConvert.PopulateObject(values, newPeriod);

            bool addResult = await _periodsService.InsertPeriod(newPeriod);

            if (addResult)
                return Ok(new { SuccessMessage = "Period added successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while adding the period" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePeriod([FromForm] IFormCollection formData)
        {
            var periodId = Convert.ToInt32(formData["key"]);
            var values = formData["values"];
            var period = await _periodsService.SelectPeriodByID(periodId);

            if (period == null)
                return NotFound(new { ErrorMessage = "Period not found" });

            JsonConvert.PopulateObject(values, period);

            bool updateResult = await _periodsService.UpdatePeriod(period);

            if (updateResult)
                return Ok(new { SuccessMessage = "Period updated successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while updating the period" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePeriod([FromForm] IFormCollection formData)
        {
            var periodId = Convert.ToInt32(formData["key"]);

            bool deleteResult = await _periodsService.DeletePeriod(periodId);

            if (deleteResult)
                return Ok(new { SuccessMessage = "Period deleted successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while deleting the period" });
        }
    }
}