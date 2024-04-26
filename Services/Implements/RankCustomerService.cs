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
    public class RankCustomerService : IRankCustomerService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseRankCustomer> _responseObject;
        private readonly RankCustomerConverter _converter;

        public RankCustomerService(AppDbContext context, ResponseObject<DataResponseRankCustomer> responseObject, RankCustomerConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponseRankCustomer> AddRankCustomer(Request_RankCustomer request)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            RankCustomer rankCustomer = new RankCustomer();
            rankCustomer.Point = request.Point;
            rankCustomer.Name = request.Name;
            rankCustomer.Description = request.Description;
            rankCustomer.IsActive = true;

            _context.rankCustomers.Add(rankCustomer);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Add rank of customer successfully!", _converter.EntityToDTO(rankCustomer));
        }

        public ResponseObject<DataResponseRankCustomer> EditRankCustomer(Request_RankCustomer request, int id)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            var rankCustomer = _context.rankCustomers.SingleOrDefault(x => x.Id == id);
            if(rankCustomer == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The rank of customer doesn't exist", null);
            }

            rankCustomer.Point = request.Point;
            rankCustomer.Name = request.Name;
            rankCustomer.Description = request.Description;

            _context.rankCustomers.Update(rankCustomer);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit rank customer successfully!", _converter.EntityToDTO(rankCustomer));
        }

        public ResponseObject<DataResponseRankCustomer> DeleteRankCustomer(int id)
        {
            var existingRankCustomer = _context.rankCustomers
                                    .Include(rankCustomer => rankCustomer.Users)
                                    .Include(rankCustomer => rankCustomer.Promotions)
                                    .SingleOrDefault(rankCustomer => rankCustomer.Id == id);

            if (existingRankCustomer == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The rank of customer is not found. Please check again!", null);
            }

            existingRankCustomer.IsActive = false;

            _context.rankCustomers.Update(existingRankCustomer);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The rank of customer has been deleted successfully!", null);
        }
    }
}
