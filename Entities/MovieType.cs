using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("MovieType")]
    public class MovieType : BaseEntity
    {
        [Required(ErrorMessage = "MovieTypeName is required")]
        public string MovieTypeName { get; set; } = "";
        public bool IsActive { get; set; } = false;

        public IEnumerable<Movie>? Movies { get; set; }
    }
}
