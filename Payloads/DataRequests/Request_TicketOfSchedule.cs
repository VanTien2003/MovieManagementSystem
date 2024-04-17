namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_TicketOfSchedule
    {
        public string Code { get; set; } = "";
        public int SeatId { get; set; }
        public double PriceTicket { get; set; }
    }
}
