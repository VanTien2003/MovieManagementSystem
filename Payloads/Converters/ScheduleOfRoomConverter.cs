using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class ScheduleOfRoomConverter
    {
        public readonly AppDbContext _context;
        public ScheduleOfRoomConverter(AppDbContext context)
        {
            _context = context;
        }

        public DataResponseScheduleOfRoom EntityToDTO(Schedule schedule)
        {
            return new DataResponseScheduleOfRoom()
            {
                Price = schedule.Price,
                StartAt = schedule.StartAt,
                EndAt = schedule.EndAt,
                Code = schedule.Code,
                MovieName = schedule.Movie.Name,
                Name = schedule.Name,
                IsActive = schedule.IsActive
            };
        }
    }
}
