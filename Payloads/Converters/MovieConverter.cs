using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class MovieConverter
    {
        private readonly AppDbContext _context;
        private readonly ScheduleOfMovieConverter _converter;
        public MovieConverter(AppDbContext context, ScheduleOfMovieConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        public DataResponseMovie EntityToDTO(Movie movie)
        {

            return new DataResponseMovie()
            {
                MovieDuration = movie.MovieDuration,
                EndTime = movie.EndTime,
                PremiereDate = movie.PremiereDate,
                Description = movie.Description,
                Director = movie.Director,
                Image = movie.Image,
                HeroImage = movie.HeroImage,
                Language = movie.Language,
                MovieTypeName = movie.MovieType.MovieTypeName,
                Name = movie.Name,
                RateDescription = movie.Rate.Description,
                Trailer = movie.Trailer,
                IsActive = movie.IsActive,
                DataResponseSchedules = _context.schedules
                                        .Where(x => x.MovieId == movie.Id)
                                        .Select(x => _converter.EntityToDTO(x))
            };
        }
    }
}
