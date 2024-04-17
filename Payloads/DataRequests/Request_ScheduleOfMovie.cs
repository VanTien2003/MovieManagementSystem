namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_ScheduleOfMovie
    {
        public double Price { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public int RoomId { get; set; }
    }
}
