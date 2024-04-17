using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("Ticket")]
    public class Ticket : BaseEntity
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = "";

        [Required(ErrorMessage = "ScheduleId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ScheduleId must be a positive value")]
        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }

        [Required(ErrorMessage = "SeatId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "SeatId must be a positive value")]
        [ForeignKey("Seat")]
        public int SeatId { get; set; }

        [Required(ErrorMessage = "PriceTicket is required")]
        [Range(0, double.MaxValue, ErrorMessage = "PriceTicket must be a non-negative value")]
        public double PriceTicket { get; set; }
        public bool IsActive { get; set; } = false;

        public Schedule? Schedule { get; set; }
        public Seat? Seat { get; set; }
        public IEnumerable<BillTicket>? BillTickets { get; set; }
    }
}
