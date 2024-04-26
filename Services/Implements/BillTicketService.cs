using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.Converters;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Services.Implements
{
    public class BillTicketService : IBillTicketService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseBillTicket> _responseObject;
        private readonly BillTicketConverter _converter;

        public BillTicketService(AppDbContext context, ResponseObject<DataResponseBillTicket> responseObject, BillTicketConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponseBillTicket> AddBillTicket(Request_BillTicket request)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            var bill = _context.bills.SingleOrDefault(x => x.Id == request.BillId);
            var ticket = _context.tickets.SingleOrDefault(x => x.Id == request.TicketId);

            if (bill == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The bill doesn't exist", null);
            }

            if (ticket == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The ticket doesn't exist", null);
            }

            BillTicket billTicket = new BillTicket();
            billTicket.Quantity = request.Quantity;
            billTicket.BillId = request.BillId;
            billTicket.TicketId = request.TicketId;

            _context.billTickets.Add(billTicket);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Add bill of ticket successfully!", _converter.EntityToDTO(billTicket));
        }

        public ResponseObject<DataResponseBillTicket> EditBillTicket(Request_BillTicket request, int id)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }
            var billTicket = _context.billTickets.SingleOrDefault(x => x.Id == id);
            var bill = _context.bills.SingleOrDefault(x => x.Id == request.BillId);
            var Ticket = _context.tickets.SingleOrDefault(x => x.Id == request.TicketId);

            if (bill == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The bill doesn't exist", null);
            }

            if (Ticket == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The Ticket doesn't exist", null);
            }

            if (billTicket == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The bill of ticket doesn't exist", null);
            }

            billTicket.Quantity = request.Quantity;
            billTicket.BillId = request.BillId;
            billTicket.TicketId = request.TicketId;

            _context.billTickets.Update(billTicket);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit bill of ticket successfully!", _converter.EntityToDTO(billTicket));
        }

        public ResponseObject<DataResponseBillTicket> DeleteBillTicket(int id)
        {
            var existingBillTicket = _context.billTickets.SingleOrDefault(x => x.Id == id);
            if(existingBillTicket == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The bill of ticket is not found. Please check again!", null);
            }

            _context.billTickets.Remove(existingBillTicket);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The bill of ticket has been deleted successfully!", null);
        }
    }
}
