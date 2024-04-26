using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("Rate")]
    public class Rate : BaseEntity
    {
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = "";

        public IEnumerable<Movie>? Movies { get; set; }
    }
}
