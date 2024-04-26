using Azure.Core;
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
    public class CinemaService : ICinemaService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseCinema> _responseObject;
        private readonly CinemaConverter _converter;

        public CinemaService(AppDbContext context, ResponseObject<DataResponseCinema> responseObject, CinemaConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponseCinema> AddCinema(Request_AddCinema request)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            Cinema cinema = new Cinema();
            cinema.Address = request.Address;
            cinema.Code = request.Code;
            cinema.Description = request.Description;
            cinema.IsActive = true;
            cinema.NameOfCinema = request.NameOfCinema;
            _context.cinemas.Add(cinema);
            _context.SaveChanges();
            if (request.AddRooms != null)
            {
                cinema.Rooms = AddRoomList(cinema.Id, request.AddRooms); 
                _context.cinemas.Update(cinema);
                _context.SaveChanges();
            }
            
            return _responseObject.ResponseSuccess("Add cinema successfully!", _converter.EntityToDTO(cinema));
        }

        private List<Room> AddRoomList(int cinemaId, List<Request_Room> requests)
        {
            var cinema = _context.cinemas.FirstOrDefault(x => x.Id == cinemaId);    
            if(cinema == null)
            {
                return null;
            }
            List<Room> list = new List<Room>();
            foreach (var request in requests)
            {
                Room room = new Room();
                room.CinemaId = cinemaId;
                room.Capacity = request.Capacity;
                room.Type = request.Type;
                room.Description = request.Description;
                room.Code = request.Code;
                room.Name = request.Name;
                room.IsActive = true;
                list.Add(room);
            }
            return list;
        }

        public ResponseObject<DataResponseCinema> EditCinema(Request_EditCinema request, int id)
        {
            var cinema = _context.cinemas.FirstOrDefault(x => x.Id == id);
            if (cinema == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The cinema doesn't exist", null);
            }
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            cinema.Address = request.Address;
            cinema.Code = request.Code;
            cinema.Description = request.Description;
            cinema.NameOfCinema = request.NameOfCinema;
            if (request.EditRooms != null)
            {
                cinema.Rooms = EditRoomList(cinema.Id, request.EditRooms);
            }
            _context.cinemas.Update(cinema);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit cinema successfully!", _converter.EntityToDTO(cinema));
        }

        private List<Room> EditRoomList(int cinemaId, List<Request_Room> requests)
        {
            var cinema = _context.cinemas.FirstOrDefault(x => x.Id == cinemaId);
            if (cinema == null)
            {
                return null;
            }

            List<Room> list = new List<Room>();
            foreach (var request in requests)
            {
                Room room = new Room();
                room.CinemaId = cinemaId;
                room.Capacity = request.Capacity;
                room.Type = request.Type;
                room.Description = request.Description;
                room.Code = request.Code;
                room.Name = request.Name;
                room.IsActive = true;
                list.Add(room);
            }
            _context.rooms.UpdateRange(list);
            _context.SaveChanges();
            return list;
        }

        
        public ResponseObject<DataResponseCinema> DeleteCinema(int id)
        {
            var existingCinema = _context.cinemas
                                    .Where(cinema => cinema.Id == id)
                                    .Include(cinema => cinema.Rooms)
                                    .SingleOrDefault();
            if (existingCinema == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The cinema is not found. Please check again!", null);
            }

            existingCinema.IsActive = false;

            _context.cinemas.Update(existingCinema);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The movie has been deleted successfully!", null);
        }

        //public ResponseObject<DataResponseCinemaRevenue> GetCinemaRevenue(DateTime startDate, DateTime endDate)
        //{
        //    try
        //    {
        //        var cinemaRevenue = _context.cinemas.Select(c => new DataResponseCinemaRevenue
        //        {
        //            CinemaName = c.NameOfCinema,
        //            TotalRevenue = _context.bills
        //                            .Where(b => b.CreateTime >= startDate && b.CreateTime <= endDate && b.IsActive)
        //                            .Join(_context.rooms, b => b.Id, r => r.Id, (b, r) => new {b , r})
        //                            .Where(x => x.r.CinemaId == c.Id)
        //                            .Sum(x => x.b.TotalMoney)
        //        }).ToList();
        //        return _responseObject.ResponseSuccess("Danh sach ", cinemaRevenue);
        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}
    }
}
