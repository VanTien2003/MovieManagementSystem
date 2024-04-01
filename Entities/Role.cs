using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        public string Code { get; set; }
        public string RoleName { get; set; }
    }
}
