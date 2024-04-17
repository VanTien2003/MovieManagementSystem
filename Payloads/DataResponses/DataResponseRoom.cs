using MovieManagementSystem.Entities;

namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseRoom
    {
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Description { get; set; } = "";
        public string CinemaName { get; set; } = "";
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public bool IsActive { get; set; }

        public IQueryable<DataResponseScheduleOfRoom>? DataResponseSchedules { get; set; }
        public IQueryable<DataResponseSeatOfRoom>? DataResponseSeats { get; set; }
    }
}
