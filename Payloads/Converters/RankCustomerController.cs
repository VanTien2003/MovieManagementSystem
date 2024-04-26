using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Payloads.Converters
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class RankCustomerController : ControllerBase
    {
        private readonly IRankCustomerService _rankCustomerService;
        public RankCustomerController(IRankCustomerService movieService)
        {
            _rankCustomerService = movieService;
        }

        [HttpPost]
        public IActionResult AddRankCustomer(Request_RankCustomer request)
        {
            return Ok(_rankCustomerService.AddRankCustomer(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRankCustomer(Request_RankCustomer request, int id)
        {
            return Ok(_rankCustomerService.EditRankCustomer(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRankCustomer(int id)
        {
            return Ok(_rankCustomerService.DeleteRankCustomer(id));
        }
    }
}
