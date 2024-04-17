using MovieManagementSystem.Entities;

namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_AddRoom
    {
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Description { get; set; } = "";
        public int CinemaId { get; set; } 
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public List<Request_ScheduleOfRoom>? AddSchedules { get; set; }
        public List<Request_Seat>? AddSeats { get; set; }
    }
}
