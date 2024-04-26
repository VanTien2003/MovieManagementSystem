using Azure.Core;
using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.Converters;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Services.Implements
{
    public class FoodService : IFoodService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseFood> _responseObject;
        private readonly FoodConverter _converter;

        public FoodService(AppDbContext context, ResponseObject<DataResponseFood> responseObject, FoodConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponseFood> AddFood(Request_Food request)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            Food food = new Food();
            food.Price = request.Price;
            food.Description = request.Description;
            food.Image = request.Image;
            food.IsActive = true;
            food.NameOfFood = request.NameOfFood;
            _context.foods.Add(food);
            _context.SaveChanges();
            
            return _responseObject.ResponseSuccess("Add food successfully!", _converter.EntityToDTO(food));
        }

        
        public ResponseObject<DataResponseFood> EditFood(Request_Food request, int id)
        {
            var food = _context.foods.FirstOrDefault(f => f.Id == id);
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }
            if (food == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The food doesn't exist", null);
            }

            food.Price = request.Price;
            food.Description = request.Description;
            food.Image = request.Image;
            food.NameOfFood = request.NameOfFood;

            _context.foods.Update(food);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit food successfully!", _converter.EntityToDTO(food));
        }
     
        public ResponseObject<DataResponseFood> DeleteFood(int id)
        {
            var existingFood = _context.foods
                                    .Where(food => food.Id == id)
                                    .Include(food => food.BillFoods)
                                    .SingleOrDefault();

            if (existingFood == null) {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The food is not found. Please check again!", null);
            }

            existingFood.IsActive = false;

            _context.foods.Update(existingFood);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The food has been deleted successfully!", null);
        }
    }
}
