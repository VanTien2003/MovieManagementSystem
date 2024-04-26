using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class TicketConverter
    {
        private readonly AppDbContext _context;

        public TicketConverter(AppDbContext context)
        {
            _context = context;
        }

        public DataResponseTicket EntityToDTO(Ticket ticket)
        {
            return new DataResponseTicket()
            {
                Code = ticket.Code,
                ScheduleName = _context.schedules.SingleOrDefault(x => x.Id == ticket.ScheduleId).Name,
                SeatNumber = _context.seats.SingleOrDefault(x => x.Id == ticket.SeatId).Number,
                PriceTicket = ticket.PriceTicket,
                IsActive = ticket.IsActive,
            };
        }
    }
}
