using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.Converters;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Services.Implements
{
    public class MovieTypeService : IMovieTypeService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseMovieType> _responseObject;
        private readonly MovieTypeConverter _converter;

        public MovieTypeService(AppDbContext context, ResponseObject<DataResponseMovieType> responseObject, MovieTypeConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponseMovieType> AddMovieType(Request_MovieType request)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            MovieType movieType = new MovieType();
            movieType.MovieTypeName = request.MovieTypeName;
            movieType.IsActive = true;

            _context.movieTypes.Add(movieType);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Add type of movie successfully!", _converter.EntityToDTO(movieType));
        }


        public ResponseObject<DataResponseMovieType> EditMovieType(Request_MovieType request, int id)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }
            var movieType = _context.movieTypes.SingleOrDefault(x => x.Id == id);
            if (movieType == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The type of movie doesn't exist", null);
            }

            movieType.MovieTypeName = request.MovieTypeName;

            _context.movieTypes.Update(movieType);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Add type of movie successfully!", _converter.EntityToDTO(movieType));
        }

        public ResponseObject<DataResponseMovieType> DeleteMovieType(int id)
        {
            var existingMovieType = _context.movieTypes
                                    .Include(movieType => movieType.Movies)
                                    .SingleOrDefault(movieType => movieType.Id == id);

            if (existingMovieType == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The type of movie is not found. Please check again!", null);
            }

            existingMovieType.IsActive = false;

            _context.movieTypes.Update(existingMovieType);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The type of movie has been deleted successfully!", null);
        }
    }
}
