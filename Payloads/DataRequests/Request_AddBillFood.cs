using MovieManagementSystem.Entities;

namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_AddBillFood
    {
        public int Quantity { get; set; }
        public int BillId { get; set; }
    }
}
