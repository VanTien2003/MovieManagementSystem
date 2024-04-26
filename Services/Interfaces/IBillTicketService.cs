using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IBillTicketService
    {
        ResponseObject<DataResponseBillTicket> AddBillTicket(Request_BillTicket request);
        ResponseObject<DataResponseBillTicket> EditBillTicket(Request_BillTicket request, int id);
        ResponseObject<DataResponseBillTicket> DeleteBillTicket(int id);
    }
}
