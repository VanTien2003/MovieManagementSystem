namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_EditUserByAdmin
    {
        public string UserName { get; set; } = "";
        public string Name { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Password { get; set; } = "";
        public int RankCustomerId { get; set; }
        public int RoleId { get; set; }
    }
}
