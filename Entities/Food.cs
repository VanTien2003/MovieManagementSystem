using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("Food")]
    public class Food : BaseEntity
    {
        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; } = "";

        [Required(ErrorMessage = "NameOfFood is required")]
        public string NameOfFood { get; set; } = "";
        public bool IsActive { get; set; } = false;

        public IEnumerable<BillFood>? BillFoods { get; set; }
    }
}
