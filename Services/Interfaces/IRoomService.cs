using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IRoomService
    {
        ResponseObject<DataResponseRoom> AddRoom(Request_AddRoom request);
        ResponseObject<DataResponseRoom> EditRoom(Request_EditRoom request, int id);
        ResponseObject<DataResponseRoom> DeleteRoom(int id);
    }
}
