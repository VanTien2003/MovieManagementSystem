using MovieManagementSystem.Payloads.DataRequests;

namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseBill
    {
        public double TotalMoney { get; set; }
        public string TradingCode { get; set; } = "";
        public DateTime CreateTime { get; set; }
        public string Name { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public int PromotionPercent { get; set; }
        public string BillStatusName { get; set; } = "";
        public bool IsActive { get; set; }
        public IQueryable<DataResponseBillFood>? DataResponseBillFoods { get; set; }
        public IQueryable<DataResponseBillTicket>? DataResponseBillTickets { get; set; }
    }
}
