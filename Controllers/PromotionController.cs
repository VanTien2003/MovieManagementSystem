using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        public PromotionController(IPromotionService movieService)
        {
            _promotionService = movieService;
        }

        [HttpPost]
        public IActionResult AddPromotion(Request_Promotion request)
        {
            return Ok(_promotionService.AddPromotion(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePromotion(Request_Promotion request, int id)
        {
            return Ok(_promotionService.EditPromotion(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePromotion(int id)
        {
            return Ok(_promotionService.DeletePromotion(id));
        }
    }
}
