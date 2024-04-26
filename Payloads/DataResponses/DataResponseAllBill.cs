namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseAllBill
    {
        public double TotalMoney { get; set; }
        public DateTime CreateTime { get; set; }
        public string MovieName { get; set; }
        public int SeatNumber { get; set; }
        public string SeatLine { get; set; }
        public string RoomName { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string ScheduleName { get; set; }
        public string CinemaName { get; set; }
        public string CinemaAddress { get; set; }
        public string TicketCode { get; set; }
    }
}
