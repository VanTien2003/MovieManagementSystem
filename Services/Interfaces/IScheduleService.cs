using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IScheduleService
    {
        ResponseObject<DataResponseSchedule> AddSchedule(Request_AddSchedule request);
        ResponseObject<DataResponseSchedule> EditSchedule(Request_EditSchedule request, int id);
        ResponseObject<DataResponseSchedule> DeleteSchedule(int id);
    }
}
