using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class TicketOfSeatConverter
    {
        private readonly AppDbContext _context;

        public TicketOfSeatConverter(AppDbContext context)
        {
            _context = context;
        }

        public DataResponseTicketOfSeat EntityToDTO(Ticket ticket)
        {
            return new DataResponseTicketOfSeat()
            {
                Code = ticket.Code,
                ScheduleName = ticket.Schedule.Name,
                PriceTicket = ticket.PriceTicket,
                IsActive = ticket.IsActive
            };
        }
    }
}
