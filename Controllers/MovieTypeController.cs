using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Implements;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class MovieTypeController : ControllerBase
    {
        private readonly IMovieTypeService _movieTypeService;
        public MovieTypeController(IMovieTypeService movieService)
        {
            _movieTypeService = movieService;
        }

        [HttpPost]
        public IActionResult AddMovieType(Request_MovieType request)
        {
            return Ok(_movieTypeService.AddMovieType(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMovieType(Request_MovieType request, int id)
        {
            return Ok(_movieTypeService.EditMovieType(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMovieType(int id)
        {
            return Ok(_movieTypeService.DeleteMovieType(id));
        }
    }
}
