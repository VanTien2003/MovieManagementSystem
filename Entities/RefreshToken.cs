using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("RefreshToken")]
    public class RefreshToken : BaseEntity
    {
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; } = "";

        [Required(ErrorMessage = "ExpiredTime is required")]
        public DateTime ExpiredTime { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive value")]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

    }
}
