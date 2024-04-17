using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class RoomOfCinemaConverter
    {
        private readonly AppDbContext _context;
        private readonly SeatConverter _seatConverter;
        private readonly ScheduleConverter _scheduleConverter;
        public RoomOfCinemaConverter(AppDbContext context, SeatConverter seatConverter, ScheduleConverter scheduleConverter)
        {
            _context = context;
            _seatConverter = seatConverter;
            _scheduleConverter = scheduleConverter;
        }

        public DataResponseRoomOfCinema EntityToDTO(Room room)
        {
            return new DataResponseRoomOfCinema()
            {
                Capacity = room.Capacity,
                Type = room.Type,
                Description = room.Description,
                //CinemaName = _context.cinemas.SingleOrDefault(x => x.Id == room.CinemaId).NameOfCinema,
                Code = room.Code,
                Name = room.Name,
                IsActive = room.IsActive
            };
        }
    }
}
