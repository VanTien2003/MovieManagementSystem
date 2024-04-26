using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;
        public BillController(IBillService billService)
        {
            _billService = billService;
        }

        [HttpPost]
        public IActionResult AddBill(Request_AddBill request)
        {
            return Ok(_billService.AddBill(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBill(Request_EditBill request, int id)
        {
            return Ok(_billService.EditBill(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBill(int id)
        {
            return Ok(_billService.DeleteBill(id));
        }

        [HttpGet("{id}")]
        public IActionResult GetBill(int id)
        {
            return Ok(_billService.GetBill(id));
        }
    }
}
