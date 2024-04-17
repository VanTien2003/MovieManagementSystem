namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_EditTicket
    {
        public string Code { get; set; } = "";
        public int ScheduleId { get; set; }
        public int SeatId { get; set; }
        public double PriceTicket { get; set; }
        public List<Request_EditBillTicket>? EditBillTickets { get; set; }
    }
}
