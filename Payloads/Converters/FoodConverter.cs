using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Services.Implements;

namespace MovieManagementSystem.Payloads.Converters
{
    public class FoodConverter
    {
        public FoodConverter(){}

        public DataResponseFood EntityToDTO(Food food)
        {
            return new DataResponseFood()
            {
                Price = food.Price,
                Description = food.Description,
                Image = food.Image,
                NameOfFood = food.NameOfFood,
                IsActive = food.IsActive
            };
        }
    }
}
