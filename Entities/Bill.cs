using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("Bill")]
    public class Bill : BaseEntity
    {

        [Required(ErrorMessage = "TotalMoney is required")]
        [Range(0, double.MaxValue, ErrorMessage = "TotalMoney must be a non-negative value")]
        public double TotalMoney { get; set; }

        [Required(ErrorMessage = "TradingCode is required")]
        public string TradingCode { get; set; } = "";
        public DateTime CreateTime { get; set; }

        [Required(ErrorMessage = "CustomerId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "PromotionId must be a positive value")]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";
        public DateTime UpdateTime { get; set; }

        [Required(ErrorMessage = "PromptionId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "PromotionId must be a positive value")]
        [ForeignKey("Promotion")]
        public int PromotionId { get; set; }
        [Required(ErrorMessage = "BillStatusId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "BillStatusId must be a positive value")]
        [ForeignKey("BillStatus")]
        public int BillStatusId { get; set; }   
        public bool IsActive { get; set; } = false;

        public User? Customer { get; set; }
        public Promotion? Promotion { get; set; }
        public BillStatus? BillStatus { get; set; }
        public IEnumerable<BillFood>? BillFoods { get; set; }
        public IEnumerable<BillTicket>? BillTickets { get; set; }
    }
}
