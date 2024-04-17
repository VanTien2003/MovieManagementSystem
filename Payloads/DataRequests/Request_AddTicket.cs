using MovieManagementSystem.Entities;

namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_AddTicket
    {
        public string Code { get; set; } = "";
        public int ScheduleId { get; set; }
        public int SeatId { get; set; }
        public double PriceTicket { get; set; }
        public List<Request_AddBillTicket>? AddBillTickets { get; set; }
    }
}
