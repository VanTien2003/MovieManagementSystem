using MovieManagementSystem.Entities;

namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseCinema
    {
        public string Address { get; set; } = "";
        public string Description { get; set; } = "";
        public string Code { get; set; } = "";
        public string NameOfCinema { get; set; } = "";
        public bool IsActive { get; set; }

        public IQueryable<DataResponseRoomOfCinema>? DataResponseRooms { get; set; }
    }
}
