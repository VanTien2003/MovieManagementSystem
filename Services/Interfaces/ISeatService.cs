using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface ISeatService
    {
        ResponseObject<DataResponseSeat> AddSeat(Request_AddSeat request);
        ResponseObject<DataResponseSeat> EditSeat(Request_EditSeat request, int id);
        ResponseObject<DataResponseSeat> DeleteSeat(int id);
    }
}
