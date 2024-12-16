using IbnSinaSystem.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IbnSinaSystem.Controllers
{
    [Authorize]
    public class DaysController : Controller
    {
        private readonly IDaysService _daysService;

        public DaysController(IDaysService daysService)
        {
            _daysService = daysService;
        }

        [HttpGet]
        public async Task<IActionResult> LoadDays()
        {
            var days = await _daysService.GetAllDays();
            return Json(days);
        }

    }
}
