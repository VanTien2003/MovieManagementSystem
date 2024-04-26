namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseBillTicket
    {
        public int Quantity { get; set; }
        public string BillName { get; set; } = "";
        public string TicketCode { get; set; } = "";
    }
}
