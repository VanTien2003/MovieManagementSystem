using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IBillService
    {
        ResponseObject<DataResponseBill> AddBill(Request_AddBill request);
        ResponseObject<DataResponseBill> EditBill(Request_EditBill request, int id);
        ResponseObject<DataResponseBill> DeleteBill(int id);

        ResponseObject<DataResponseAllBill> GetBill(int id);
    }
}
