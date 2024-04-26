using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost]
        public ActionResult AddSchedule(Request_AddSchedule request)
        {
            return Ok(_scheduleService.AddSchedule(request));
        }

        [HttpPut("{id}")]
        public IActionResult EditSchedule(Request_EditSchedule request, int id)
        {
            return Ok(_scheduleService.EditSchedule(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSchedule(int id)
        {
            return Ok(_scheduleService.DeleteSchedule(id));
        }
    }
}
