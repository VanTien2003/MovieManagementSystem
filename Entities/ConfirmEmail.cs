using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("ConfirmEmail")]
    public class ConfirmEmail : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime ExpiredTime { get; set; }
        public string ConfirmCode { get; set; }
        public bool IsConfirm { get; set; } = false;
    }
}
