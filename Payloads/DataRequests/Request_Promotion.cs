using MovieManagementSystem.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_Promotion
    {
        public int Percent { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; } = "";
        public DateTime EndTime { get; set; }
        public string Description { get; set; } = "";
        public string Name { get; set; } = "";
        public int RankCustomerId { get; set; }
    }
}
