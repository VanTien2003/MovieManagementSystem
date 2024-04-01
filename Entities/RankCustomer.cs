using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("RankCustomer")]
    public class RankCustomer : BaseEntity
    {
        public int Point { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
