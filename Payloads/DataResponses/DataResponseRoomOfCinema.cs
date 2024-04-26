namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseRoomOfCinema
    {
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Description { get; set; } = "";
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public bool IsActive { get; set; }
    }
}
