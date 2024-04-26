using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("SeatType")]
    public class SeatType : BaseEntity
    {
        [Required(ErrorMessage = "NameType is required")]
        public string NameType { get; set; } = "";

        public IEnumerable<Seat>? Seats { get; set; }
    }
}
