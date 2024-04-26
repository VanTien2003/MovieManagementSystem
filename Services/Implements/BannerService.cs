using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.Converters;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Services.Implements
{
    public class BannerService : IBannerService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseBanner> _responseObject;
        private readonly BannerConverter _converter;
        public BannerService(AppDbContext context, ResponseObject<DataResponseBanner> responseObject, BannerConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponseBanner> AddBanner(Request_Banner request)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            Banner banner = new Banner();
            banner.ImageUrl = request.ImageUrl;
            banner.Title = request.Title;
            _context.banners.Add(banner);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Add banner successfully!", _converter.EntityToDTO(banner));
        }

        public ResponseObject<DataResponseBanner> EditBanner(Request_Banner request, int id)
        {
            var banner = _context.banners.FirstOrDefault(b => b.Id == id);
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }
            if (banner == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The banner doesn't exist", null);
            }

            banner.ImageUrl = request.ImageUrl;
            banner.Title = request.Title;
            _context.banners.Update(banner);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit banner successfully!", _converter.EntityToDTO(banner));
        }

        public ResponseObject<DataResponseBanner> DeleteBanner(int id)
        {
            var existingBanner = _context.banners.SingleOrDefault(b => b.Id == id); 
            if(existingBanner == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The banner is not found. Please check again!", null);
            }

            _context.banners.Remove(existingBanner);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The banner has been deleted successfully!", null);
        }
    }
}
