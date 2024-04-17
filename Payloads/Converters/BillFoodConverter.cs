using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class BillFoodConverter
    {
        private readonly AppDbContext _context;

        public BillFoodConverter(AppDbContext context)
        {
            _context = context;
        }

        public DataResponseBillFood EntityToDTO(BillFood billFood)
        {
            return new DataResponseBillFood()
            {
                Quantity = billFood.Quantity,
                BillName = _context.bills.SingleOrDefault(x => x.Id == billFood.BillId).Name,
                FoodName = _context.foods.SingleOrDefault(x => x.Id == billFood.FoodId).NameOfFood
            };
        }
    }
}
