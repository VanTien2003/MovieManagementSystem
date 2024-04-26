using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface ITicketService
    {
        ResponseObject<DataResponseTicket> AddTicket(Request_Ticket request);
        ResponseObject<DataResponseTicket> EditTicket(Request_Ticket request, int id);
        ResponseObject<DataResponseTicket> DeleteTicket(int id);
    }
}
