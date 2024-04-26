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

        public ResponseObject<DataResponseSeat> AddSeat(Request_Seat request)
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
            
            return _responseObject.ResponseSuccess("Add seat successfully!", _converter.EntityToDTO(seat));
        }

        public ResponseObject<DataResponseSeat> EditSeat(Request_Seat request, int id)
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

            _context.seats.Update(seat);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit seat successfully!", _converter.EntityToDTO(seat));
        }

        public ResponseObject<DataResponseSeat> DeleteSeat(int id)
        {
            var existingSeat = _context.seats
                                    .Include(seat => seat.Tickets)
                                    .SingleOrDefault(seat => seat.Id == id);
            if (existingSeat == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The seat is not found. Please check again!", null);
            }

            existingSeat.IsActive = false;
        
            _context.seats.Update(existingSeat);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The seat has been deleted successfully!", null);
        }

    }
}
