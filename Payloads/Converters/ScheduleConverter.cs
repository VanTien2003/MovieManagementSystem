using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class ScheduleConverter
    {
        public readonly AppDbContext _context;
        public readonly TicketOfScheduleConverter _converter;
        public ScheduleConverter(AppDbContext context, TicketOfScheduleConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        public DataResponseSchedule EntityToDTO(Schedule schedule)
        {
            return new DataResponseSchedule()
            {
                Price = schedule.Price,
                StartAt = schedule.StartAt,
                EndAt = schedule.EndAt,
                Code = schedule.Code,
                MovieName = _context.movies.SingleOrDefault(x => x.Id == schedule.MovieId).Name,
                Name = schedule.Name,
                RoomName = _context.rooms.SingleOrDefault(x => x.Id == schedule.RoomId).Name,
                IsActive = schedule.IsActive,
                DataResponseTickets = _context.tickets
                                        .Where(x => x.ScheduleId == schedule.Id)
                                        .Select(x => _converter.EntityToDTO(x))
                                            
            };
        }
    }
}
