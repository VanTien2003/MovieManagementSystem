using MovieManagementSystem.Entities;

namespace MovieManagementSystem.Payloads.DataResponses
{
    public class DataResponseFood
    {
        public double Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string NameOfFood { get; set; }
        public bool IsActive { get; set; }

        public IQueryable<DataResponseBillFood>? DataResponseBillFoods { get; set; }
    }
}
