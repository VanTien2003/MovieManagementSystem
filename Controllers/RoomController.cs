using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost]
        public IActionResult AddRoom(Request_AddRoom request)
        {
            return Ok(_roomService.AddRoom(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRoom(Request_EditRoom request, int id)
        {
            return Ok(_roomService.EditRoom(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            return Ok(_roomService.DeleteRoom(id));
        }
    }
}
