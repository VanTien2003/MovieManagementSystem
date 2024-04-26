using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Implements;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpPost]
        public IActionResult AddBanner(Request_Banner request)
        {
            return Ok(_bannerService.AddBanner(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBanner(Request_Banner request, int id)
        {
            return Ok(_bannerService.EditBanner(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBanner(int id)
        {
            return Ok(_bannerService.DeleteBanner(id));
        }
    }
}
