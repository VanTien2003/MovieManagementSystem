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
    public class CinemaController : ControllerBase
    {
        private readonly ICinemaService _cinemaService;
        public CinemaController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        [HttpPost]
        public IActionResult AddCinema(Request_AddCinema request)
        {
            return Ok(_cinemaService.AddCinema(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCinema(Request_EditCinema request, int id)
        {
            return Ok(_cinemaService.EditCinema(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCinema(int id)
        {
            return Ok(_cinemaService.DeleteCinema(id));
        }
    }
}
