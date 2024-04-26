using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IMovieTypeService
    {
        ResponseObject<DataResponseMovieType> AddMovieType(Request_MovieType request);
        ResponseObject<DataResponseMovieType> EditMovieType(Request_MovieType request, int id);
        ResponseObject<DataResponseMovieType> DeleteMovieType(int id);
    }
}
