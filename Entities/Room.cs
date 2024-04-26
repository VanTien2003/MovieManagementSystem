using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("Room")]
    public class Room : BaseEntity
    {
        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be a positive value")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Type is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Type must be a positive value")]
        public int Type { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "CinemaId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CinemaId must be a positive value")]
        [ForeignKey("Cinema")]
        public int CinemaId { get; set; }

        [ForeignKey("CinemaId")]
        public Cinema? Cinema { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = "";

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";
        public bool IsActive { get; set; } = false;
        public IEnumerable<Schedule>? Schedules { get; set; }
        public IEnumerable<Seat>? Seats { get; set; }
    }
}
