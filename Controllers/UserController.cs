using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/api/auth/register")]
        public IActionResult Register([FromForm] Request_Register request)
        {
            return Ok(_userService.Register(request));
        }


        [HttpPost("/api/auth/login")]
        public IActionResult Login([FromForm] Request_Login request)
        {
            return Ok(_userService.Login(request));
        }

        [HttpGet("/api/auth/get-all")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }
    }
}
