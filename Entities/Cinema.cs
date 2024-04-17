using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("Cinema")]
    public class Cinema : BaseEntity
    {

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = "";

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = "";

        [Required(ErrorMessage = "NameOfCinema is required")]
        public string NameOfCinema { get; set; } = "";
        public bool IsActive { get; set; } = false;

        public IEnumerable<Room>? Rooms { get; set; }
    }
}
