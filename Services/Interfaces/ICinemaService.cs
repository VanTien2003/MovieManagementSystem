using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface ICinemaService
    {
        ResponseObject<DataResponseCinema> AddCinema(Request_AddCinema request);
        ResponseObject<DataResponseCinema> EditCinema(Request_EditCinema request, int id);
        ResponseObject<DataResponseCinema> DeleteCinema(int id);
    }
}
