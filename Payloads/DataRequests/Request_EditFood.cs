namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_EditFood
    {
        public double Price { get; set; }
        public string Description { get; set; } = "";
        public string Image { get; set; } = "";
        public string NameOfFood { get; set; } = "";

        public List<Request_EditBillFood>? EditBillFoods { get; set; }
    }
}
