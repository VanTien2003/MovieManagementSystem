using System.ComponentModel.DataAnnotations;

namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseRankCustomer
    {
        public int RankPoint { get; set; }
        public string Description { get; set; } = "";
        public string RankName { get; set; } = "";
        public bool IsASctive { get; set; }
    }
}
