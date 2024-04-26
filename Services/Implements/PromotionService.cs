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
    public class PromotionService : IPromotionService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponsePromotion> _responseObject;
        private readonly PromotionConverter _converter;

        public PromotionService(AppDbContext context, ResponseObject<DataResponsePromotion> responseObject, PromotionConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponsePromotion> AddPromotion(Request_Promotion request)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            var rankCustomer = _context.rankCustomers.SingleOrDefault(x => x.Id == request.RankCustomerId);
            if(rankCustomer == null) {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The rank of customer doesn't exist", null);
            }

            Promotion promotion = new Promotion();
            promotion.Percent = request.Percent;
            promotion.Quantity = request.Quantity;
            promotion.Type = request.Type;
            promotion.StartTime = DateTime.Now;
            promotion.EndTime = request.EndTime;
            promotion.Description = request.Description;
            promotion.Name = request.Name;
            promotion.IsActive = true;
            promotion.RankCustomerId = request.RankCustomerId;

            _context.promotions.Add(promotion);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Add promotion successfully!", _converter.EntityToDTO(promotion));
        }

        public ResponseObject<DataResponsePromotion> EditPromotion(Request_Promotion request, int id)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            var rankCustomer = _context.rankCustomers.SingleOrDefault(x => x.Id == request.RankCustomerId);
            var promotion = _context.promotions.SingleOrDefault(x => x.Id == id);
            if (rankCustomer == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The rank of customer doesn't exist", null);
            }
            if(promotion == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The promotion doesn't exist", null);
            }

            promotion.Percent = request.Percent;
            promotion.Quantity = request.Quantity;
            promotion.Type = request.Type;
            promotion.EndTime = request.EndTime;
            promotion.Description = request.Description;
            promotion.Name = request.Name;
            promotion.RankCustomerId = request.RankCustomerId;

            _context.promotions.Update(promotion);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit promotion successfully!", _converter.EntityToDTO(promotion));
        }

        public ResponseObject<DataResponsePromotion> DeletePromotion(int id)
        {
            var existingPromotion = _context.promotions
                                        .Include(promotion => promotion.Bills)
                                        .SingleOrDefault(promotion => promotion.Id == id);  

            if(existingPromotion == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The promotion is not found. Please check again!", null);
            }

            existingPromotion.IsActive = false;

            _context.promotions.Update(existingPromotion);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The promotion has been deleted successfully!", null);
        }
    }
}
