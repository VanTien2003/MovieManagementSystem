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

        public ResponseObject<DataResponseFood> AddFood(Request_AddFood request)
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

            if (request.AddBillFoods != null)
            {
                food.BillFoods = AddBillFoodList(food.Id, request.AddBillFoods);
                _context.foods.Update(food);
                _context.SaveChanges();
            }
            
            return _responseObject.ResponseSuccess("Add food successfully!", _converter.EntityToDTO(food));
        }

        public List<BillFood> AddBillFoodList(int foodId, List<Request_AddBillFood> requests)
        {
            var food = _context.foods.FirstOrDefault(f => f.Id == foodId);
            if(food == null)
            {
                return null;
            }

            List<BillFood> list = new List<BillFood>();
            foreach (var request in requests)
            {
                var bill = _context.bills.SingleOrDefault(x => x.Id == request.BillId);
                if(bill == null)
                {
                    return null;
                }

                BillFood billFood = new BillFood();
                billFood.Quantity = request.Quantity;
                billFood.BillId = request.BillId;
                billFood.FoodId = foodId;
                list.Add(billFood);
            }
            return list;
        }
        public ResponseObject<DataResponseFood> EditFood(Request_EditFood request, int id)
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

            if (request.EditBillFoods != null)
            {
                food.BillFoods = EditBillFoodList(food.Id, request.EditBillFoods);
            }
            _context.foods.Update(food);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit food successfully!", _converter.EntityToDTO(food));
        }

        public List<BillFood> EditBillFoodList(int foodId, List<Request_EditBillFood> requests)
        {
            var food = _context.foods.FirstOrDefault(f => f.Id == foodId);
            if (food == null)
            {
                return null;
            }

            List<BillFood> list = new List<BillFood>();
            foreach (var request in requests)
            {
                BillFood billFood = new BillFood();
                billFood.Quantity = request.Quantity;
                billFood.BillId = request.BillId;
                billFood.FoodId = foodId;
                list.Add(billFood);
            }
            return list;
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

            if(existingFood.BillFoods != null)
            {
                _context.billFoods.RemoveRange(existingFood.BillFoods); 
            }

            _context.foods.Remove(existingFood);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The food has been deleted successfully!", null);
        }
    }
}
