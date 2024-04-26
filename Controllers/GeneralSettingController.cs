using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Implements;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class GeneralSettingController : ControllerBase
    {
        private readonly IGeneralSettingService _generalSettingService;
        public GeneralSettingController(IGeneralSettingService generalSettingService)
        {
            _generalSettingService = generalSettingService;
        }

        [HttpPost]
        public IActionResult AddGeneralSetting(Request_GeneralSetting request)
        {
            return Ok(_generalSettingService.AddGeneralSetting(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGeneralSetting(Request_GeneralSetting request, int id)
        {
            return Ok(_generalSettingService.EditGeneralSetting(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGeneralSetting(int id)
        {
            return Ok(_generalSettingService.DeleteGeneralSetting(id));
        }
    }
}
