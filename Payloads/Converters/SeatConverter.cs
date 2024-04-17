using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class SeatConverter
    {
        private readonly AppDbContext _context;
        private readonly TicketOfSeatConverter _converter;

        public SeatConverter(AppDbContext context, TicketOfSeatConverter converter)
        {
            _context = context;
            _converter = converter;
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
                SeatTypeName = _context.seatTypes.SingleOrDefault(x => x.Id == seat.SeatTypeId).NameType,
                DataReponseTickets = _context.tickets  
                                        .Where(x => x.SeatId == seat.Id)
                                        .Select(x => _converter.EntityToDTO(x))  
            };
        }
    }
}
