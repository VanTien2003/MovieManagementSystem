using System.ComponentModel.DataAnnotations;

namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_RankCustomer
    {
        public int Point { get; set; }
        public string Description { get; set; } = "";
        public string Name { get; set; } = "";
    }
}
