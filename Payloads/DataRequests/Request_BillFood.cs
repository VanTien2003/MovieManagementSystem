namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_BillFood
    {
        public int Quantity { get; set; }   
        public int BillId { get; set; }
        public int FoodId { get; set; }
    }
}
