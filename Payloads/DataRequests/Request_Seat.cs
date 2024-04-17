namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_Seat
    {
        public int Number { get; set; }
        public int SeatStatusId { get; set; }
        public string Line { get; set; } = "";
        public int SeatTypeId { get; set; }
    }
}
