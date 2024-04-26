using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;
        public SeatController(ISeatService movieService)
        {
            _seatService = movieService;
        }

        [HttpPost]
        public IActionResult AddSeat(Request_Seat request)
        {
            return Ok(_seatService.AddSeat(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSeat(Request_Seat request, int id)
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
