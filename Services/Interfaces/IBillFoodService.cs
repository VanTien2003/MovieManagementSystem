using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IBillFoodService
    {
        ResponseObject<DataResponseBillFood> AddBillFood(Request_BillFood request);
        ResponseObject<DataResponseBillFood> EditBillFood(Request_BillFood request, int id);
        ResponseObject<DataResponseBillFood> DeleteBillFood(int id);
    }
}
