using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("User")]
    [Index("UserName", IsUnique = true)]
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "Point is required")]
        public int Point { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } = "";

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "PhoneNumber is required")]
        [RegularExpression(@"^\+?\d{10,11}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "RankCustomerId is required")]
        [ForeignKey("RankCustomer")]
        public int RankCustomerId { get; set; }
        public RankCustomer? RankCustomer { get; set; }

        [Required(ErrorMessage = "UserStatusId is required")]
        [ForeignKey("UserStatus")]
        public int UserStatusId { get; set; }
        public UserStatus? UserStatus { get; set; }

        [Required(ErrorMessage = "RoleId is required")]
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public bool IsActive { get; set; } = false;

        public IEnumerable<ConfirmEmail>? ConfirmEmails { get; set; }
        public IEnumerable<RefreshToken>? RefreshTokens { get; set; }
        public IEnumerable<Bill>? Bills { get; set; }
    }
}
