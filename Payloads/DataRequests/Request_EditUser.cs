namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_EditUser
    {
        public string UserName { get; set; } = "";
        public string Name { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
