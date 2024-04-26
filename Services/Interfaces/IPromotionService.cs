using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IPromotionService
    {
        ResponseObject<DataResponsePromotion> AddPromotion(Request_Promotion request);
        ResponseObject<DataResponsePromotion> EditPromotion(Request_Promotion request, int id);
        ResponseObject<DataResponsePromotion> DeletePromotion(int id);
    }
}
