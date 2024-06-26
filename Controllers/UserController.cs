﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Route("Api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Register([FromBody] Request_Register request)
        {
            return Ok(_userService.Register(request));
        }


        [HttpPost]
        public IActionResult Login([FromBody] Request_Login request)
        {
            return Ok(_userService.Login(request));
        }

        [HttpPost]
        [Authorize]
        public IActionResult ConfirmAccount(string confirmationCode)
        {
            return Ok(_userService.ConfirmAccount(confirmationCode));
        }

        [HttpPost]
        [Authorize]
        public IActionResult SendConfirmationCode(string email)
        {
            return Ok(_userService.SendConfirmationCode(email));
        }

        [HttpPut]
        [Authorize]
        public IActionResult ResetPassword(string resetCode, string newPassword)
        {
            return Ok(_userService.ResetPassword(resetCode, newPassword));
        }

        [HttpPost]
        [Authorize]
        public IActionResult RenewAccessToken(Request_RenewAccessToken request)
        {
            return Ok(_userService.RenewAccessToken(request));
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateUser(Request_EditUser request, int id)
        {
            return Ok(_userService.EditUser(request, id));
        }      
    }
}
