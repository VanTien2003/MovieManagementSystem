using MovieManagementSystem.Entities;

namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_Food
    {
        public double Price { get; set; }
        public string Description { get; set; } = "";
        public string Image { get; set; } = "";
        public string NameOfFood { get; set; } = "";
    }
}
