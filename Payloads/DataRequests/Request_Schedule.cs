namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_ScheduleOfRoom
    {
        public double Price { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Code { get; set; } = "";
        public int MovieId { get; set; }
        public string Name { get; set; } = "";
    }
}
