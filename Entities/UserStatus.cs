using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("UserStatus")]
    public class UserStatus : BaseEntity
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = "";

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";

        public IEnumerable<User>? Users { get; set; }
    }
}
