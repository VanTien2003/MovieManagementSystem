using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class SeatOfRoomConverter
    {
        private readonly AppDbContext _context;

        public SeatOfRoomConverter(AppDbContext context)
        {
            _context = context;
        }

        public DataResponseSeatOfRoom EntityToDTO(Seat seat)
        {
            return new DataResponseSeatOfRoom()
            {
                Number = seat.Number,
                SeatStatusName = seat.SeatStatus.NameStatus,
                Line = seat.Line,
                RoomName = seat.Room.Name,
                IsActive = seat.IsActive,
                SeatTypeName = seat.SeatType.NameType
            };
        }
    }
}
