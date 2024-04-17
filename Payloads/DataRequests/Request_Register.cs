using MovieManagementSystem.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieManagementSystem.Payloads.DataRequests
{
    public class Request_Register
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } = "";
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = "";
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = "";
        public int RankCustomerId { get; set; }
        public int UserStatusId { get; set; }
    }
}
