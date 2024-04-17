namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_EditBillTicket
    {
        public int Quantity { get; set; }
        public int BillId { get; set; }
        public int TicketId { get; set; }
    }
}
