namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseScheduleOfMovie
    {
        public double Price { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string RoomName { get; set; } = "";
        public bool IsActive { get; set; } = false;
    }
}
