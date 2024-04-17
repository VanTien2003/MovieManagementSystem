namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseTicketOfSeat
    {
        public string Code { get; set; } = "";
        public string ScheduleName { get; set; } = "";
        public double PriceTicket { get; set; }
        public bool IsActive { get; set; }
    }
}
