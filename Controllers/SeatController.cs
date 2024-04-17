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
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;
        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpPost]
        public IActionResult AddSeat(Request_AddSeat request)
        {
            return Ok(_seatService.AddSeat(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSeat(Request_EditSeat request, int id)
        {
            return Ok(_seatService.EditSeat(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSeat(int id)
        {
            return Ok(_seatService.DeleteSeat(id));
        }
    }
}
