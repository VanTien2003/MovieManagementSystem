using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("ConfirmEmail")]
    public class ConfirmEmail : BaseEntity
    {
        [Required(ErrorMessage = "UserId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive value")]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime ExpiredTime { get; set; }
        [Required(ErrorMessage = "ConfirmCode is required")]
        public string ConfirmCode { get; set; } = "";
        public bool IsConfirm { get; set; } = false;
    }
}
