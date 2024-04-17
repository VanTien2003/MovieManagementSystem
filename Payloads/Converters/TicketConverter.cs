using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class TicketConverter
    {
        private readonly AppDbContext _context;
        private readonly BillTicketConverter _converter;

        public TicketConverter(AppDbContext context, BillTicketConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        public DataResponseTicket EntityToDTO(Ticket ticket)
        {
            return new DataResponseTicket()
            {
                Code = ticket.Code,
                ScheduleName = ticket.Schedule.Name,
                SeatNumber = ticket.Seat.Number,
                PriceTicket = ticket.PriceTicket,
                IsActive = ticket.IsActive,
                DataResponseBillTickets = _context.billTickets
                                            .Where(x => x.TicketId == ticket.Id)
                                            .Select(x => _converter.EntityToDTO(x))
            };
        }
    }
}
