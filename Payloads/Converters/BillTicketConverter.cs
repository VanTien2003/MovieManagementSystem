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
                TicketCode = _context.tickets.SingleOrDefault(x => x.Id == billTicket.TicketId).Code,
                BillName = _context.bills.SingleOrDefault(x => x.Id == billTicket.BillId).Name
            };
        }
    }
}
