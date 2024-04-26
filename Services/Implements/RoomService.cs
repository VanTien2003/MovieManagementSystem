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
    public class RoomService : IRoomService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseRoom> _responseObject;
        private readonly RoomConverter _converter;

        public RoomService(AppDbContext context, ResponseObject<DataResponseRoom> responseObject, RoomConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponseRoom> AddRoom(Request_AddRoom request)
        {
            var cinema = _context.cinemas.SingleOrDefault(x => x.Id == request.CinemaId);
            if(cinema == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The cinema doesn't exist", null);
            }
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            Room room = new Room();
            room.CinemaId = request.CinemaId;
            room.Capacity = request.Capacity;
            room.Type = request.Type;
            room.Description = request.Description;
            room.Code = request.Code;
            room.Name = request.Name;
            room.IsActive = true;
            _context.rooms.Add(room);
            _context.SaveChanges();

            if (request.AddSchedules != null)
            {
                room.Schedules = AddScheduleList(room.Id, request.AddSchedules);
                _context.rooms.Update(room);
                _context.SaveChanges();
            }
            if (request.AddSeats != null)
            {
                room.Seats = AddSeatList(room.Id, request.AddSeats);
                _context.rooms.Update(room);
                _context.SaveChanges();
            }
            
            return _responseObject.ResponseSuccess("Add room successfully!", _converter.EntityToDTO(room));
        }

        private List<Seat> AddSeatList(int roomId, List<Request_Seat> requests)
        {
            var room = _context.rooms.FirstOrDefault(x => x.Id == roomId);
            if (room == null)
            {
                return null;
            }
            List<Seat> list = new List<Seat>();
            foreach (var request in requests)
            {
                var seatStatus = _context.seatStatus.SingleOrDefault(x => x.Id == request.SeatStatusId);
                var seatType = _context.seatTypes.SingleOrDefault(x => x.Id == request.SeatTypeId);
                if (seatStatus == null || seatType == null)
                {
                    return null;
                }

                Seat seat = new Seat();
                seat.Number = request.Number;
                seat.SeatStatusId = request.SeatStatusId;
                seat.Line = request.Line;
                seat.RoomId = roomId;
                seat.SeatTypeId = request.SeatTypeId;
                seat.IsActive = true;
                seat.Tickets = null;
                list.Add(seat);
            }
            _context.seats.AddRange(list);
            _context.SaveChanges();
            return list;
        }

        private List<Schedule> AddScheduleList(int roomId, List<Request_ScheduleOfRoom> requests)
        {
            var room = _context.rooms.FirstOrDefault(x => x.Id == roomId);
            if (room == null)
            {
                return null;
            }

            List<Schedule> list = new List<Schedule>();
            foreach (var request in requests)
            {
                var movie = _context.movies.FirstOrDefault(x => x.Id == request.MovieId);
                if (movie == null)
                {
                    return null;
                }

                Schedule schedule = new Schedule();
                schedule.Price = request.Price;
                schedule.StartAt = request.StartAt;
                schedule.EndAt = request.EndAt;
                schedule.Code = request.Code;
                schedule.MovieId = request.MovieId;
                schedule.Name = request.Name;
                schedule.IsActive = true;
                schedule.RoomId = roomId;
                list.Add(schedule);
            }
            _context.schedules.AddRange(list);
            _context.SaveChanges();
            return list;
        }

        
        public ResponseObject<DataResponseRoom> EditRoom(Request_EditRoom request, int id)
        {
            var room = _context.rooms.SingleOrDefault(x => x.Id == id);
            var cinema = _context.cinemas.SingleOrDefault(x => x.Id == request.CinemaId);
            if (cinema == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The cinema doesn't exist", null);
            }
            if (room == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The room doesn't exist", null);
            }
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            room.CinemaId = request.CinemaId;
            room.Capacity = request.Capacity;
            room.Type = request.Type;
            room.Description = request.Description;
            room.Code = request.Code;
            room.Name = request.Name;
            if (request.EditSchedules != null)
            {
                room.Schedules = EditScheduleList(room.Id, request.EditSchedules);
            }
            if (request.EditSeats != null)
            {
                room.Seats = EditSeatList(room.Id, request.EditSeats);
            }
            _context.rooms.Update(room);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit room successfully!", _converter.EntityToDTO(room));
        }

        private List<Seat> EditSeatList(int roomId, List<Request_Seat> requests)
        {
            var room = _context.rooms.FirstOrDefault(x => x.Id == roomId);
            if (room == null)
            {
                return null;
            }
            List<Seat> list = new List<Seat>();
            foreach (var request in requests)
            {
                var seatStatus = _context.seatStatus.SingleOrDefault(x => x.Id == request.SeatStatusId);
                var seatType = _context.seatTypes.SingleOrDefault(x => x.Id == request.SeatTypeId);
                if (seatStatus == null || seatType == null)
                {
                    return null;
                }

                Seat seat = new Seat();
                seat.Number = request.Number;
                seat.SeatStatusId = request.SeatStatusId;
                seat.Line = request.Line;
                seat.RoomId = roomId;
                seat.SeatTypeId = request.SeatTypeId;
                seat.IsActive = true;
                list.Add(seat);
            }
            _context.seats.UpdateRange(list);
            _context.SaveChanges();
            return list;
        }

        private List<Schedule> EditScheduleList(int roomId, List<Request_ScheduleOfRoom> requests)
        {
            var room = _context.rooms.FirstOrDefault(x => x.Id == roomId);
            if (room == null)
            {
                return null;
            }

            List<Schedule> list = new List<Schedule>();
            foreach (var request in requests)
            {
                var movie = _context.movies.FirstOrDefault(x => x.Id == request.MovieId);
                if (movie == null)
                {
                    return null;
                }

                Schedule schedule = new Schedule();
                schedule.Price = request.Price;
                schedule.StartAt = request.StartAt;
                schedule.EndAt = request.EndAt;
                schedule.Code = request.Code;
                schedule.MovieId = request.MovieId;
                schedule.Name = request.Name;
                schedule.RoomId = roomId;
                schedule.IsActive = true;
                list.Add(schedule);
            }
            _context.schedules.UpdateRange(list);
            _context.SaveChanges();
            return list;
        }

        public ResponseObject<DataResponseRoom> DeleteRoom(int id)
        {
            var existingRoom = _context.rooms
                                    .Include(room => room.Schedules)
                                    .Include(room => room.Seats)
                                    .SingleOrDefault(room => room.Id == id);

            if (existingRoom == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The room is not found. Please check again!", null);
            }

            existingRoom.IsActive = false;
            
            _context.rooms.Update(existingRoom);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The room has been deleted successfully!", null);
        }
    }
}
