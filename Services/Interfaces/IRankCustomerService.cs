using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IRankCustomerService
    {
        ResponseObject<DataResponseRankCustomer> AddRankCustomer(Request_RankCustomer request);
        ResponseObject<DataResponseRankCustomer> EditRankCustomer(Request_RankCustomer request, int id);
        ResponseObject<DataResponseRankCustomer> DeleteRankCustomer(int id);
    }
}
