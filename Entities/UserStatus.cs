using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("UserStatus")]
    public class UserStatus : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
