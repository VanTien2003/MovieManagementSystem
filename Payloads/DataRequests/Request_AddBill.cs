using MovieManagementSystem.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_AddBill
    {
        public string TradingCode { get; set; } = "";
        public string Name { get; set; } = "";
        public int CustomerId { get; set; }
        public int PromotionId { get; set; }
        public int BillStatusId { get; set; }
        public List<Request_BillFoodOfBill>? AddBillFoods { get; set; }
        public List<Request_BillTicketOfBill>? AddBillTickets { get; set; }
    }
}
