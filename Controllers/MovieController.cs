using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public IActionResult GetPopularMovies(int limit)
        {
            return Ok(_movieService.GetPopularMovies(limit));
        }

        [HttpGet]
        public IActionResult GetMoviesByCinemaRoomAndSeatStatus(int cinemaId, int roomId, string seatStatus)
        {
            return Ok(_movieService.GetMoviesByCinemaRoomAndSeatStatus(cinemaId, roomId, seatStatus));
        }

        [HttpPost]
        public IActionResult AddMovie(Request_AddMovie request)
        {
            return Ok(_movieService.AddMovie(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMovie(Request_EditMovie request, int id) {
            return Ok(_movieService.EditMovie(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            return Ok(_movieService.DeleteMovie(id));
        }
    }
}
