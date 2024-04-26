using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;
        public FoodController(IFoodService movieService)
        {
            _foodService = movieService;
        }

        [HttpPost]
        public IActionResult AddFood(Request_Food request)
        {
            return Ok(_foodService.AddFood(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFood(Request_Food request, int id)
        {
            return Ok(_foodService.EditFood(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFood(int id)
        {
            return Ok(_foodService.DeleteFood(id));
        }
    }
}
