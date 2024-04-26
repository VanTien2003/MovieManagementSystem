using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class BillFoodController : ControllerBase
    {
        private readonly IBillFoodService _billFoodService;
        public BillFoodController(IBillFoodService movieService)
        {
            _billFoodService = movieService;
        }

        [HttpPost]
        public IActionResult AddBillFood(Request_BillFood request)
        {
            return Ok(_billFoodService.AddBillFood(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBillFood(Request_BillFood request, int id)
        {
            return Ok(_billFoodService.EditBillFood(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBillFood(int id)
        {
            return Ok(_billFoodService.DeleteBillFood(id));
        }
    }
}
