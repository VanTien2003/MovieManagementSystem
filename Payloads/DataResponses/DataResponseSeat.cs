using MovieManagementSystem.Entities;

namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseSeat
    {
        public int Number { get; set; }
        public string SeatStatusName { get; set; } = "";
        public string Line { get; set; } = "";
        public string RoomName { get; set; } = "";
        public bool IsActive { get; set; }
        public string SeatTypeName { get; set; } = "";

        public IQueryable<DataResponseTicketOfSeat>? DataReponseTickets { get; set; }
    }
}
