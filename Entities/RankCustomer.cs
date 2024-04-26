using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("RankCustomer")]
    public class RankCustomer : BaseEntity
    {
        [Required(ErrorMessage = "Point is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Point must be a non-negative value")]
        public int Point { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";
        public bool IsActive { get; set; } = false;

        public IEnumerable<Promotion>? Promotions { get; set; }
        public IEnumerable<User>? Users { get; set; }
    }
}
