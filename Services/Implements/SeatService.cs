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
    public class SeatService : ISeatService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseSeat> _responseObject;
        private readonly SeatConverter _converter;

        public SeatService(AppDbContext context, ResponseObject<DataResponseSeat> responseObject, SeatConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponseSeat> AddSeat(Request_AddSeat request)
        {
            var seatStatus = _context.seatStatus.SingleOrDefault(x => x.Id == request.SeatStatusId);
            var seatType = _context.seatTypes.SingleOrDefault(x => x.Id == request.SeatTypeId);
            var room = _context.rooms.SingleOrDefault(x => x.Id == request.RoomId); 
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }
            if (seatStatus == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The seat status doesn't exist", null);
            }
            if (seatType == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The seat type doesn't exist", null);
            }
            if (room == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The room doesn't exist", null);
            }

            Seat seat = new Seat();
            seat.Number = request.Number;
            seat.SeatStatusId = request.SeatStatusId;
            seat.Line = request.Line;
            seat.RoomId = request.RoomId;
            seat.SeatTypeId = request.SeatTypeId;
            seat.IsActive = true;
            _context.seats.Add(seat);
            _context.SaveChanges();

            if (request.AddTickets != null)
            {
                seat.Tickets = AddTicketListWithSeat(seat.Id, request.AddTickets);
                _context.seats.Update(seat);
                _context.SaveChanges();
            }
            
            return _responseObject.ResponseSuccess("Add seat successfully!", _converter.EntityToDTO(seat));
        }

        public List<Ticket> AddTicketListWithSeat(int seatId, List<Request_Ticket> requests)
        {
            var seat = _context.seats.FirstOrDefault(x => x.Id == seatId);
            if (seat == null)
            {
                return null;
            }

            List<Ticket> list = new List<Ticket>();
            foreach (var request in requests)
            {
                var schedule = _context.schedules.FirstOrDefault(x => x.Id == request.ScheduleId);
                if (seat == null)
                {
                    return null;
                }

                Ticket ticket = new Ticket();
                ticket.Code = request.Code;
                ticket.ScheduleId = request.ScheduleId;
                ticket.SeatId = seatId;
                ticket.PriceTicket = request.PriceTicket;
                ticket.IsActive = true;
                list.Add(ticket);
            }
            _context.tickets.AddRange(list);
            _context.SaveChanges();
            return list;
        }

        public ResponseObject<DataResponseSeat> EditSeat(Request_EditSeat request, int id)
        {
            var seat = _context.seats.SingleOrDefault(x => x.Id == id);
            var seatStatus = _context.seatStatus.SingleOrDefault(x => x.Id == request.SeatStatusId);
            var seatType = _context.seatTypes.SingleOrDefault(x => x.Id == request.SeatTypeId);
            var room = _context.rooms.SingleOrDefault(x => x.Id == request.RoomId);
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }
            if (seatStatus == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The seat status doesn't exist", null);
            }
            if (seatType == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The seat type doesn't exist", null);
            }
            if (room == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The room doesn't exist", null);
            }
            if (seat == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The seat doesn't exist", null);
            }

            seat.Number = request.Number;
            seat.SeatStatusId = request.SeatStatusId;
            seat.Line = request.Line;
            seat.RoomId = request.RoomId;
            seat.SeatTypeId = request.SeatTypeId;

            if (request.EditTickets != null)
            {
                seat.Tickets = EditTicketListWithSeat(seat.Id, request.EditTickets);
            }
            _context.seats.Update(seat);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit seat successfully!", _converter.EntityToDTO(seat));
        }

        private List<Ticket> EditTicketListWithSeat(int seatId, List<Request_Ticket> requests)
        {
            var seat = _context.seats.FirstOrDefault(x => x.Id == seatId);
            if (seat == null)
            {
                return null;
            }

            List<Ticket> list = new List<Ticket>();
            foreach (var request in requests)
            {
                var schedule = _context.schedules.FirstOrDefault(x => x.Id == request.ScheduleId);
                if (schedule == null)
                {
                    return null;
                }

                Ticket ticket = new Ticket();
                ticket.Code = request.Code;
                ticket.ScheduleId = request.ScheduleId;
                ticket.SeatId = seatId;
                ticket.PriceTicket = request.PriceTicket;
                ticket.IsActive = true;
                list.Add(ticket);
            }
            return list;
        }

        public ResponseObject<DataResponseSeat> DeleteSeat(int id)
        {
            var existingSeat = _context.seats
                                    .Where(seat => seat.Id == id)
                                    .Include(seat => seat.Tickets)
                                    .SingleOrDefault();
            if (existingSeat == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The seat is not found. Please check again!", null);
            }

            if(existingSeat.Tickets != null)
            {
                _context.tickets.RemoveRange(existingSeat.Tickets);
            }
        
            _context.seats.Remove(existingSeat);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The seat has been deleted successfully!", null);
        }

    }
}
