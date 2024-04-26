using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IBannerService
    {
        ResponseObject<DataResponseBanner> AddBanner(Request_Banner request);
        ResponseObject<DataResponseBanner> EditBanner(Request_Banner request, int id);
        ResponseObject<DataResponseBanner> DeleteBanner(int id);
    }
}
