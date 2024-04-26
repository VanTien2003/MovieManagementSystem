using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService movieService)
        {
            _ticketService = movieService;
        }

        [HttpPost]
        public IActionResult AddTicket(Request_Ticket request)
        {
            return Ok(_ticketService.AddTicket(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTicket(Request_Ticket request, int id)
        {
            return Ok(_ticketService.EditTicket(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTicket(int id)
        {
            return Ok(_ticketService.DeleteTicket(id));
        }
    }
}
