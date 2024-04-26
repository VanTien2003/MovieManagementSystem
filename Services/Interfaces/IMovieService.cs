using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IMovieService
    {
        List<DataResponseMovie> GetPopularMovies(int limit);
        List<DataResponseMovie> GetMoviesByCinemaRoomAndSeatStatus(int cinemaId, int roomId, string seatStatus);
        ResponseObject<DataResponseMovie> AddMovie(Request_AddMovie request);
        ResponseObject<DataResponseMovie> EditMovie(Request_EditMovie request, int id);
        ResponseObject<DataResponseMovie> DeleteMovie(int id);
    }
}
