namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_EditBill
    {
        public string TradingCode { get; set; } = "";
        public string Name { get; set; } = "";
        public int CustomerId { get; set; }
        public int PromotionId { get; set; }
        public int BillStatusId { get; set; }
        public List<Request_BillFoodOfBill>? EditBillFoods { get; set; }
        public List<Request_BillTicketOfBill>? EditBillTickets { get; set; }
    }
}
