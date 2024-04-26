namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_EditSchedule
    {
        public double Price { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Code { get; set; } = "";
        public int MovieId { get; set; }
        public string Name { get; set; } = "";
        public int RoomId { get; set; }
        public List<Request_TicketOfSchedule>? EditTickets { get; set; }
    }
}
