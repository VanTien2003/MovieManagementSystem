using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class BillTicketController : ControllerBase
    {
        private readonly IBillTicketService _billTicketService;
        public BillTicketController(IBillTicketService movieService)
        {
            _billTicketService = movieService;
        }

        [HttpPost]
        public IActionResult AddBillTicket(Request_BillTicket request)
        {
            return Ok(_billTicketService.AddBillTicket(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBillTicket(Request_BillTicket request, int id)
        {
            return Ok(_billTicketService.EditBillTicket(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBillTicket(int id)
        {
            return Ok(_billTicketService.DeleteBillTicket(id));
        }
    }
}
