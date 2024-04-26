namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseTicketOfSchedule
    {
        public string Code { get; set; } = "";
        public int SeatNumber { get; set; }
        public double PriceTicket { get; set; }
        public bool IsActive { get; set; }
    }
}
