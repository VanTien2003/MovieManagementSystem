using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("Seat")]
    public class Seat : BaseEntity
    {
        [Required(ErrorMessage = "Number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Number must be a positive value")]
        public int Number { get; set; }

        [Required(ErrorMessage = "SeatStatusId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "SeatStatusId must be a positive value")]
        [ForeignKey("SeatStatus")]
        public int SeatStatusId { get; set; }

        [Required(ErrorMessage = "Line is required")]
        public string Line { get; set; } = "";

        [Required(ErrorMessage = "RoomId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "RoomId must be a positive value")]
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public bool IsActive { get; set; } = false;

        [Required(ErrorMessage = "SeatTypeId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "SeatTypeId must be a positive value")]
        [ForeignKey("SeatType")]
        public int SeatTypeId { get; set; }

        public Room? Room { get; set; }
        public SeatStatus? SeatStatus { get; set; }
        public SeatType? SeatType { get; set; }

        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
