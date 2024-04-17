using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class CinemaConverter
    {
        private readonly AppDbContext _context;
        private readonly RoomOfCinemaConverter _converter;

        public CinemaConverter(AppDbContext context, RoomOfCinemaConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        public DataResponseCinema EntityToDTO(Cinema cinema)
        {
            return new DataResponseCinema()
            {
                Address = cinema.Address,
                Description = cinema.Description,
                Code = cinema.Code,
                NameOfCinema = cinema.NameOfCinema,
                IsActive = cinema.IsActive,
                DataResponseRooms = _context.rooms
                                        .Where(x => x.CinemaId == cinema.Id)
                                        .Select(x => _converter.EntityToDTO(x))
            };
        }
    }
}
