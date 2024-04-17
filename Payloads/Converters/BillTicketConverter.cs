using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class BillTicketConverter
    {
        private readonly AppDbContext _context;
        public BillTicketConverter(AppDbContext context)
        {
            _context = context;
        }

        public DataResponseBillTicket EntityToDTO(BillTicket billTicket)
        {
            return new DataResponseBillTicket()
            {
                Quantity = billTicket.Quantity,
                BillName = billTicket.Bill.Name,
                TicketCode = billTicket.Ticket.Code
            };
        }
    }
}
