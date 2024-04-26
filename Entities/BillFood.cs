using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("BillFood")]
    public class BillFood : BaseEntity
    {
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive value")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "BillId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "BillId must be a positive value")]
        [ForeignKey("Bill")]
        public int BillId { get; set; }
        [Required(ErrorMessage = "FoodId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "FoodId must be a positive value")]
        [ForeignKey("Food")]
        public int FoodId { get; set; }

        public Bill? Bill { get; set; }
        public Food? Food { get; set; }
    }
}
