namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_EditRoom
    {
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Description { get; set; } = "";
        public int CinemaId { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public List<Request_ScheduleOfRoom>? EditSchedules { get; set; }
        public List<Request_Seat>? EditSeats { get; set; }
    }
}
