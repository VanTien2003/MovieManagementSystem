using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.Responses;
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
        public IActionResult Register([FromBody] Request_Register request)
        {
            return Ok(_userService.Register(request));
        }


        [HttpPost("/api/auth/login")]
        public IActionResult Login([FromBody] Request_Login request)
        {
            return Ok(_userService.Login(request));
        }

        [HttpGet("/api/auth/get-all")]
        [Authorize(Roles = "Admin")]
        public ActionResult GetAll([FromQuery] Pagination pagination)
        {
            return Ok(_userService.GetAll(pagination));
        }

        [HttpPost("/api/auth/confirm-account")]
        [Authorize]
        public IActionResult ConfirmAccount(string confirmationCode)
        {
            return Ok(_userService.ConfirmAccount(confirmationCode));
        }

        [HttpPost("/api/auth/send-confirmation-code")]
        [Authorize]
        public IActionResult SendConfirmationCode(string email)
        {
            return Ok(_userService.SendConfirmationCode(email));
        }

        [HttpPost("/api/auth/forgot-password")]
        [Authorize]
        public IActionResult ResetPassword(string resetCode, string newPassword)
        {
            return Ok(_userService.ResetPassword(resetCode, newPassword));
        }

        [HttpPost("/api/auth/change-password")]
        [Authorize]
        public IActionResult ChangePassword([FromBody] Request_ChangePassword request)
        {
            // Lấy ID của người dùng đang đăng nhập từ các thông tin xác thực
            int userId = int.Parse(HttpContext.User.FindFirst("Id").Value);
            if(userId != 0)
            {
                // Tìm người dùng trong cơ sở dữ liệu
                var result = _userService.ChangePassword(request, userId);

                if (result.Data)
                {
                    return Ok(new ResponseObject<bool>(StatusCodes.Status200OK, "Đổi mật khẩu thành công!", true));
                }
                else
                {
                    return BadRequest(new ResponseObject<bool>(StatusCodes.Status400BadRequest, "Đổi mật khẩu thất bại!", false));
                }
            }else
            {
                return BadRequest(new ResponseObject<bool>(StatusCodes.Status400BadRequest, "Người dùng chưa được xác thực", false));
            }
        }
    }
}
