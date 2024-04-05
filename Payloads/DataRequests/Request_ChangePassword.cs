namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_ChangePassword
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
