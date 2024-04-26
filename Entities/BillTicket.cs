using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace MovieManagementSystem.Entities
{
    [Table("BillTicket")]
    public class BillTicket : BaseEntity
    {
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive value")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "BillId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "BillId must be a positive value")]
        [ForeignKey("Bill")]
        public int BillId { get; set; }

        [Required(ErrorMessage = "TicketId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "TicketId must be a positive value")]
        [ForeignKey("Ticket")]
        public int TicketId { get; set; }

        public Bill? Bill { get; set; }
        public Ticket? Ticket { get; set; }
    }
}
