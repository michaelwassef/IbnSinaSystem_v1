using IbnSinaSystem.IServices;
using IbnSinaSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IbnSinaSystem.Controllers
{
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly IRoomsService _roomsService;

        public RoomsController(IRoomsService roomsService)
        {
            _roomsService = roomsService;
        }

        public IActionResult RoomsPage()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole != "Admin") { return RedirectToAction("AccessDenied", "Access"); }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadRooms()
        {
            var rooms = await _roomsService.GetAllRooms();
            return Json(rooms);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom([FromForm] IFormCollection formData)
        {
            var values = formData["values"];
            var newRoom = new RoomsModel();
            JsonConvert.PopulateObject(values, newRoom);

            bool addResult = await _roomsService.InsertRoom(newRoom);

            if (addResult)
                return Ok(new { SuccessMessage = "Room added successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while adding the room" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoom([FromForm] IFormCollection formData)
        {
            var roomId = Convert.ToInt32(formData["key"]);
            var values = formData["values"];
            var room = await _roomsService.SelectRoomByID(roomId);

            if (room == null)
                return NotFound(new { ErrorMessage = "Room not found" });

            JsonConvert.PopulateObject(values, room);

            bool updateResult = await _roomsService.UpdateRoom(room);

            if (updateResult)
                return Ok(new { SuccessMessage = "Room updated successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while updating the room" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRoom([FromForm] IFormCollection formData)
        {
            var roomId = Convert.ToInt32(formData["key"]);

            bool deleteResult = await _roomsService.DeleteRoom(roomId);

            if (deleteResult)
                return Ok(new { SuccessMessage = "Room deleted successfully" });
            else
                return BadRequest(new { ErrorMessage = "Error occurred while deleting the room" });
        }
    }
}
