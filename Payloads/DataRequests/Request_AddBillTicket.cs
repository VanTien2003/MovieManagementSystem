using MovieManagementSystem.Entities;

namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_AddBillTicket
    {
        public int Quantity { get; set; }
        public int BillId { get; set; }
        public int TicketId { get; set; }
    }
}
