using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MovieManagementSystem.DataContext;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;
using MovieManagementSystem.Services.Implements;
using MovieManagementSystem.Services.Interfaces;
using System.Security.Claims;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public PaymentController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreatePaymentUrl(int billId)
        {
            // Lấy ID của người dùng đang đăng nhập từ các thông tin xác thực
            if (!int.TryParse(HttpContext.User.FindFirstValue("Id"), out int userId) || userId == 0)
            {
                return BadRequest(new ResponseObject<bool>(StatusCodes.Status400BadRequest, "User is not authenticated!", false));
            }
            return Ok( await _vnPayService.CreatePaymentUrl(billId, HttpContext, userId));
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallback(string url)
        {
            try
            {
                // Tạo đối tượng Uri từ URL được truyền vào
                var uri = new Uri(url);

                // Phân tích URL để lấy các tham số truy vấn
                var query = QueryHelpers.ParseQuery(uri.Query);

                // Chuyển đổi Dictionary<string, StringValues> sang IQueryCollection
                var queryCollection = new QueryCollection(query);

                // Gọi phương thức PaymentExecute và truyền vào IQueryCollection
                var response = await _vnPayService.PaymentExecute(queryCollection);

                if (response.Status == StatusCodes.Status200OK)
                {
                    // Xử lý dữ liệu thanh toán thành công ở đây
                    return Ok(response);
                }
                else
                {
                    // Xử lý dữ liệu thanh toán không thành công ở đây
                    return BadRequest("Invalid payment data");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }
    }
}
