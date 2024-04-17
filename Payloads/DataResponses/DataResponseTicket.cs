using MovieManagementSystem.Entities;

namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseTicket
    {
        public string Code { get; set; } = "";
        public string ScheduleName { get; set; } = "";
        public int SeatNumber { get; set; }
        public double PriceTicket { get; set; }
        public bool IsActive { get; set; }
        public IQueryable<DataResponseBillTicket>? DataResponseBillTickets { get; set; }
    }
}
