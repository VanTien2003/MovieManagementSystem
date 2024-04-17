using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("Promotion")]
    public class Promotion : BaseEntity
    {
        [Required(ErrorMessage = "Percent is required")]
        [Range(0, 100, ErrorMessage = "Percent must be between 0 and 100")]
        public int Percent { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive value")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } = "";

        [Required(ErrorMessage = "StartTime is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "EndTime is required")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";
        public bool IsActive { get; set; } = false;

        [ForeignKey("RankCustomer")]
        [Required(ErrorMessage = "RankCustomerId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "RankCustomerId must be a positive value")]
        public int RankCustomerId { get; set; }

        public RankCustomer? RankCustomer { get; set; }
        public IEnumerable<Bill>? Bills { get; set; }
    }
}
