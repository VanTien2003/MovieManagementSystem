using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class SeatConverter
    {
        private readonly AppDbContext _context;

        public SeatConverter(AppDbContext context)
        {
            _context = context;
        }

        public DataResponseSeat EntityToDTO(Seat seat)
        {
            return new DataResponseSeat()
            {
                Number = seat.Number,
                SeatStatusName = _context.seatStatus.SingleOrDefault(x => x.Id == seat.SeatStatusId).NameStatus,
                Line = seat.Line,
                RoomName = _context.rooms.SingleOrDefault(x => x.Id == seat.RoomId).Name,
                IsActive = seat.IsActive,
                SeatTypeName = _context.seatTypes.SingleOrDefault(x => x.Id == seat.SeatTypeId).NameType
            };
        }
    }
}
