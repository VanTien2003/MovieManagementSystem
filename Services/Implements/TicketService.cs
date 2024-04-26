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
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseTicket> _responseObject;
        private readonly TicketConverter _converter;

        public TicketService(AppDbContext context, ResponseObject<DataResponseTicket> responseObject, TicketConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponseTicket> AddTicket(Request_Ticket request)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            var schedule = _context.schedules.SingleOrDefault(x => x.Id == request.ScheduleId);
            var seat = _context.seats.SingleOrDefault(x => x.Id == request.SeatId);
            if(schedule == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The schedule doesn't exist", null);
            }

            if(seat == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The seat doesn't exist", null);
            }

            Ticket ticket = new Ticket();
            ticket.Code = request.Code;
            ticket.PriceTicket = request.PriceTicket;
            ticket.ScheduleId = request.ScheduleId;
            ticket.SeatId = request.SeatId;
            ticket.IsActive = true;

            _context.tickets.Add(ticket);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Add ticket successfully!", _converter.EntityToDTO(ticket));
        }

        public ResponseObject<DataResponseTicket> EditTicket(Request_Ticket request, int id)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            var ticket = _context.tickets.SingleOrDefault(x => x.Id == id);
            var schedule = _context.schedules.SingleOrDefault(x => x.Id == request.ScheduleId);
            var seat = _context.seats.SingleOrDefault(x => x.Id == request.SeatId);
            if (schedule == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The schedule doesn't exist", null);
            }

            if (seat == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The seat doesn't exist", null);
            }
            if(ticket == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The ticket doesn't exist", null);
            }

            ticket.Code = request.Code;
            ticket.PriceTicket = request.PriceTicket;
            ticket.ScheduleId = request.ScheduleId;
            ticket.SeatId = request.SeatId;

            _context.tickets.Update(ticket);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit ticket successfully!", _converter.EntityToDTO(ticket));
        }

        public ResponseObject<DataResponseTicket> DeleteTicket(int id)
        {
            var existingTicket = _context.tickets
                                    .Include(ticket => ticket.BillTickets)
                                    .SingleOrDefault(ticket => ticket.Id == id);

            if(existingTicket == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The ticket is not found. Please check again!", null);
            }

            existingTicket.IsActive = false;

            _context.tickets.Update(existingTicket);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The ticket has been deleted successfully!", null);
        }
    }
}
