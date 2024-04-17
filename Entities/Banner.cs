using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("Banner")]
    public class Banner : BaseEntity
    {
        [Required(ErrorMessage = "ImageUrl is required")]
        public string ImageUrl { get; set; } = "";

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = "";
    }
}
