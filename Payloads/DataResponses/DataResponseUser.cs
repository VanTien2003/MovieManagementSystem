using MovieManagementSystem.Entities;

namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseUser
    {
        public int Point { get; set; }
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string RankCustomerName { get; set; } = "";
        public string UserStatusName { get; set; } = "";
        public bool IsActive { get; set; }
        public string RoleName { get; set; } = "";
    }
}
