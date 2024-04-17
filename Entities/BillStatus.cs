using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("BillStatus")]
    public class BillStatus : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";
        public IEnumerable<Bill>? Bills { get; set; }
    }
}
