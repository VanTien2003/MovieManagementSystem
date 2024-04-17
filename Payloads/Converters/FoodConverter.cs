using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Services.Implements;

namespace MovieManagementSystem.Payloads.Converters
{
    public class FoodConverter
    {
        private readonly AppDbContext _context;
        private readonly BillFoodConverter _converter;

        public FoodConverter(AppDbContext context, BillFoodConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        public DataResponseFood EntityToDTO(Food food)
        {
            return new DataResponseFood()
            {
                Price = food.Price,
                Description = food.Description,
                Image = food.Image,
                NameOfFood = food.NameOfFood,
                IsActive = food.IsActive,
                DataResponseBillFoods = _context.billFoods
                                            .Where(x => x.FoodId == food.Id)
                                            .Select(x => _converter.EntityToDTO(x))
            };
        }
    }
}
