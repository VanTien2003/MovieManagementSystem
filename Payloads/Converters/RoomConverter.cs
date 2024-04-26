using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;
using System;

namespace MovieManagementSystem.Payloads.Converters
{
    public class RoomConverter
    {
        private readonly AppDbContext _context;
        private readonly SeatOfRoomConverter _seatConverter;
        private readonly ScheduleOfRoomConverter _scheduleConverter;
        public RoomConverter(AppDbContext context, SeatOfRoomConverter seatConverter, ScheduleOfRoomConverter scheduleConverter)
        {
            _context = context;
            _seatConverter = seatConverter;
            _scheduleConverter = scheduleConverter;
        }

        public DataResponseRoom EntityToDTO(Room room)
        {
            return new DataResponseRoom()
            {
                Capacity = room.Capacity,
                Type = room.Type,
                Description = room.Description,
                CinemaName = _context.cinemas.SingleOrDefault(x => x.Id == room.CinemaId).NameOfCinema,
                Code = room.Code,
                Name = room.Name,
                IsActive = room.IsActive,
                DataResponseSchedules = _context.schedules
                                        .Where(x => x.RoomId == room.Id)
                                        .Select(x => _scheduleConverter.EntityToDTO(x)),
                DataResponseSeats = _context.seats
                                        .Where(x => x.RoomId ==  room.Id)
                                        .Select(x => _seatConverter.EntityToDTO(x))
            };
        }
    }
}
