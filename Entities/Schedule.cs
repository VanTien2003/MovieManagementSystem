using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("Schedule")]
    public class Schedule : BaseEntity
    {
        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value")]
        public double Price { get; set; }

        [Required(ErrorMessage = "StartAt is required")]
        public DateTime StartAt { get; set; }

        [Required(ErrorMessage = "EndAt is required")]
        public DateTime EndAt { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = "";

        [Required(ErrorMessage = "MovieId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "MovieId must be a positive value")]
        [ForeignKey("Movie")]
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "RoomId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "RoomId must be a positive value")]
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public bool IsActive { get; set; } = false;

        public Movie? Movie { get; set; }
        public Room? Room { get; set; }
        public IEnumerable<Ticket>? Tickets { get; set; }
    }
}
