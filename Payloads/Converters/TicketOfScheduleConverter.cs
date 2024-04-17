using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class TicketOfScheduleConverter
    {
        private readonly AppDbContext _context;

        public TicketOfScheduleConverter(AppDbContext context)
        {
            _context = context;
        }

        public DataResponseTicketOfSchedule EntityToDTO(Ticket ticket)
        {
            return new DataResponseTicketOfSchedule()
            {
                Code = ticket.Code,
                SeatNumber = ticket.Seat != null ? ticket.Seat.Number : 0,
                PriceTicket = ticket.PriceTicket,
                IsActive = ticket.IsActive
            };
        }
    }
}
