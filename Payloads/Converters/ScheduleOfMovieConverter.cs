using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class ScheduleOfMovieConverter
    {
        public readonly AppDbContext _context;
        public ScheduleOfMovieConverter(AppDbContext context)
        {
            _context = context;
        }

        public DataResponseScheduleOfMovie EntityToDTO(Schedule schedule)
        {
            return new DataResponseScheduleOfMovie()
            {
                Price = schedule.Price,
                StartAt = schedule.StartAt,
                EndAt = schedule.EndAt,
                Code = schedule.Code,
                Name = schedule.Name,
                RoomName = schedule.Room.Name,
                IsActive = schedule.IsActive,
            };
        }
    }
}
