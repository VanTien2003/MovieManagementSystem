using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.Converters;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Services.Implements
{
    public class BillFoodService : IBillFoodService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseBillFood> _responseObject;
        private readonly BillFoodConverter _converter;

        public BillFoodService(AppDbContext context, ResponseObject<DataResponseBillFood> responseObject, BillFoodConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponseBillFood> AddBillFood(Request_BillFood request)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            var bill = _context.bills.SingleOrDefault(x => x.Id == request.BillId);
            var food = _context.foods.SingleOrDefault(x => x.Id == request.FoodId);

            if (bill == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The bill doesn't exist", null);
            }

            if(food == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The food doesn't exist", null);
            }

            BillFood billFood = new BillFood();
            billFood.Quantity = request.Quantity;
            billFood.BillId = request.BillId;
            billFood.FoodId = request.FoodId;

            _context.billFoods.Add(billFood);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Add bill of food successfully!", _converter.EntityToDTO(billFood));
        }

        public ResponseObject<DataResponseBillFood> EditBillFood(Request_BillFood request, int id)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }
            var billFood = _context.billFoods.SingleOrDefault(x => x.Id == id);
            var bill = _context.bills.SingleOrDefault(x => x.Id == request.BillId);
            var food = _context.foods.SingleOrDefault(x => x.Id == request.FoodId);

            if (bill == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The bill doesn't exist", null);
            }

            if (food == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The food doesn't exist", null);
            }

            if(billFood == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The bill of food doesn't exist", null);
            }

            billFood.Quantity = request.Quantity;
            billFood.BillId = request.BillId;
            billFood.FoodId = request.FoodId;

            _context.billFoods.Update(billFood);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit bill of food successfully!", _converter.EntityToDTO(billFood));
        }

        public ResponseObject<DataResponseBillFood> DeleteBillFood(int id)
        {
            var existingBillFood = _context.billFoods.SingleOrDefault(b => b.Id == id);

            if (existingBillFood == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The bill of food is not found. Please check again!", null);
            }

            _context.billFoods.Remove(existingBillFood);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The bill of food has been deleted successfully!", null);
        }
    }
}
