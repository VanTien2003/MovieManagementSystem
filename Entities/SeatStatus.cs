using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("SeatStatus")]
    public class SeatStatus : BaseEntity
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = "";

        [Required(ErrorMessage = "NameStatus is required")]
        public string NameStatus { get; set; } = "";

        public IEnumerable<Seat>? Seats { get; set; }
    }
}
