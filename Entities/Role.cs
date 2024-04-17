using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = "";

        [Required(ErrorMessage = "RoleName is required")]
        public string RoleName { get; set; } = "";

        public IEnumerable<User>? Users { get; set; }
    }
}
