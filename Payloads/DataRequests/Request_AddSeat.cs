using MovieManagementSystem.Entities;

namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_AddSeat
    {
        public int Number { get; set; }
        public int SeatStatusId { get; set; }
        public string Line { get; set; } = "";
        public int RoomId { get; set; }
        public int SeatTypeId { get; set; }

        public List<Request_Ticket>? AddTickets { get; set; }
    }
}
