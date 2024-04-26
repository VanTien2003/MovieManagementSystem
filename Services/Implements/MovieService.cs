using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.Converters;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;
using MovieManagementSystem.Services.Interfaces;
using System.Linq;

namespace MovieManagementSystem.Services.Implements
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseMovie> _responseObject;
        private readonly MovieConverter _converter;

        public MovieService(AppDbContext context, ResponseObject<DataResponseMovie> responseObject, MovieConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponseMovie> AddMovie(Request_AddMovie request)
        {
            var movieType = _context.movieTypes.FirstOrDefault(x => x.Id == request.MovieTypeId);
            var rate = _context.rates.FirstOrDefault(x => x.Id == request.RateId);
            if(movieType == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The movie doesn't exist", null);
            }
            if (rate == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The rate doesn't exist", null);
            }

            Movie movie = new Movie();
            movie.MovieDuration = request.MovieDuration;
            movie.EndTime = request.EndTime;
            movie.PremiereDate = request.PremiereDate;
            movie.Description = request.Description;
            movie.Director = request.Director;
            movie.Image = request.Image;
            movie.HeroImage = request.HeroImage;
            movie.Language = request.Language;
            movie.MovieTypeId = request.MovieTypeId;
            movie.Name = request.Name;
            movie.RateId = request.RateId;
            movie.Trailer = request.Trailer;
            movie.IsActive = true;
            _context.movies.Add(movie);
            _context.SaveChanges();

            if (request.AddSchedules != null)
            {
                movie.Schedules = AddScheduleList(movie.Id, request.AddSchedules);
                //_context.movies.Update(movie);
                _context.SaveChanges();
            }
            
            return _responseObject.ResponseSuccess("Add movie successfully!", _converter.EntityToDTO(movie));
        }

        private List<Schedule> AddScheduleList(int movieId, List<Request_ScheduleOfMovie> requests)
        {
            var movie = _context.movies.FirstOrDefault(x => x.Id == movieId);
            if(movie == null)
            {
                return null;
            }

            List<Schedule> list = new List<Schedule>();
            foreach (var request in requests)
            {
                var room = _context.rooms.FirstOrDefault(x => x.Id == request.RoomId);
                if (room == null)
                {
                    return null;
                }

                Schedule schedule = new Schedule();
                schedule.Price = request.Price;
                schedule.StartAt = request.StartAt;
                schedule.EndAt = request.EndAt;
                schedule.Code = request.Code;
                schedule.MovieId = movieId;
                schedule.Name = request.Name;
                schedule.IsActive = true;
                schedule.RoomId = request.RoomId;
                schedule.Tickets = null;
                list.Add(schedule);
            }
            _context.schedules.AddRange(list);
            _context.SaveChanges();
            return list;
        }

        public ResponseObject<DataResponseMovie> EditMovie(Request_EditMovie request, int id)
        {
            var movie = _context.movies.FirstOrDefault(x => x.Id == id);
            var movieType = _context.movieTypes.FirstOrDefault(x => x.Id == request.MovieTypeId);
            var rate = _context.rates.FirstOrDefault(x => x.Id == request.RateId);
            if (movieType == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Movie type cannot be blank", null);
            }
            if (movie == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The movie doesn't exist", null);
            }
            if (rate == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Rate cannot be blank", null);
            }

            movie.MovieDuration = request.MovieDuration;
            movie.EndTime = request.EndTime;
            movie.PremiereDate = request.PremiereDate;
            movie.Description = request.Description;
            movie.Director = request.Director;
            movie.Image = request.Image;
            movie.HeroImage = request.HeroImage;
            movie.Language = request.Language;
            movie.MovieTypeId = request.MovieTypeId;
            movie.Name = request.Name;
            movie.RateId = request.RateId;
            movie.Trailer = request.Trailer;

            if (request.EditSchedules != null)
            {
                movie.Schedules = EditScheduleList(movie.Id, request.EditSchedules);
            }
            _context.movies.Update(movie);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Edit movie successfully!", _converter.EntityToDTO(movie));
        }

        private List<Schedule> EditScheduleList(int movieId, List<Request_ScheduleOfMovie> requests)
        {
            var movie = _context.movies.FirstOrDefault(x => x.Id == movieId);
            if (movie == null)
            {
                return null;
            }

            List<Schedule> list = new List<Schedule>();
            foreach (var request in requests)
            {
                var room = _context.rooms.FirstOrDefault(x => x.Id == request.RoomId);
                if (room == null)
                {
                    return null;
                }

                Schedule schedule = new Schedule();
                schedule.Price = request.Price;
                schedule.StartAt = request.StartAt;
                schedule.EndAt = request.EndAt;
                schedule.Code = request.Code;
                schedule.MovieId = movieId;
                schedule.Name = request.Name;
                schedule.RoomId = request.RoomId;
                schedule.IsActive = true;
                list.Add(schedule);
            }

            _context.schedules.UpdateRange(list);
            _context.SaveChanges();
            return list;
        }

        public ResponseObject<DataResponseMovie> DeleteMovie(int id)
        {
            var existingMovie = _context.movies
                                    .Where(movie => movie.Id == id)
                                    .Include(movie => movie.Schedules)
                                    .SingleOrDefault();
            if (existingMovie == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The movie is not found. Please check again!", null);
            }

            existingMovie.IsActive = false;

            _context.movies.Update(existingMovie);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The movie has been deleted successfully!", null);
        }

        public List<DataResponseMovie> GetPopularMovies(int limit)
        {
            try
            {
                // Truy vấn cơ sở dữ liệu để lấy danh sách các bộ phim, sắp xếp chúng theo số lượng vé đã đặt, và giới hạn số lượng kết quả trả về theo giá trị limit
                var popularMovies = _context.movies
                    .OrderByDescending(m => m.Schedules.Sum(s => s.Tickets.Count()))
                    .Take(limit)
                    .ToList();

                var result = popularMovies.Select(x => _converter.EntityToDTO(x)).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public List<DataResponseMovie> GetMoviesByCinemaRoomAndSeatStatus(int cinemaId, int roomId, string seatStatus)
        {
            try
            {
                // Lấy danh sách các lịch chiếu phim cho một rạp và một phòng cụ thể
                var schedules = _context.schedules
                    .Include(s => s.Movie)
                    .Where(s => s.Room.CinemaId == cinemaId && s.RoomId == roomId)
                    .ToList();

                // Lấy danh sách các ghế trong phòng và trạng thái của chúng
                var seats = _context.seats
                    .Where(seat => seat.RoomId == roomId)
                    .ToList();

                // Lọc ra các ghế theo trạng thái mong muốn
                var filteredSeats = seats.Where(seat => seat.SeatStatus.NameStatus.ToLower().Equals(seatStatus.ToLower())).ToList();

                // Hiển thị danh sách phim mà lịch chiếu có sẵn trong phòng đó
                var movies = new List<Movie>();
                foreach (var schedule in schedules)
                {
                    if (filteredSeats.Any(seat => seat.RoomId == schedule.RoomId))
                    {
                        movies.Add(schedule.Movie);
                    }
                }

                var result = movies.Select(x => _converter.EntityToDTO(x)).ToList();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
