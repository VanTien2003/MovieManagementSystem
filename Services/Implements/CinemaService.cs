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

        //private void AddSeatList(int roomId, List<Request_AddSeat> requests)
        //{
        //    var room = _context.rooms.FirstOrDefault(x => x.Id == roomId);
        //    if (room == null)
        //    {
        //        return;
        //    }

        //    foreach (var request in requests)
        //    {
        //        var seatStatus = _context.seatStatus.SingleOrDefault(x => x.Id == request.SeatStatusId);
        //        var seatType = _context.seatTypes.SingleOrDefault(x => x.Id == request.SeatTypeId);
        //        if(seatStatus == null || seatType == null)
        //        {
        //            return;
        //        }

        //        Seat seat = new Seat();
        //        seat.Number = request.Number;
        //        seat.SeatStatusId = request.SeatStatusId;
        //        seat.Line = request.Line;
        //        seat.RoomId = roomId;
        //        seat.SeatTypeId = request.SeatTypeId;
        //        seat.IsActive = true;

        //        if(request.AddTickets != null)
        //        {
        //            AddTicketListWithSeat(seat.Id, request.AddTickets);
        //            _context.SaveChanges();
        //        }
        //        _context.seats.Add(seat);
        //    }
        //}

        //public void AddTicketListWithSeat(int seatId, List<Request_AddTicket> requests)
        //{
        //    var seat = _context.seats.FirstOrDefault(x => x.Id == seatId);
        //    if (seat == null)
        //    {
        //        return;
        //    }

        //    foreach (var request in requests)
        //    {
        //        var schedule = _context.schedules.FirstOrDefault(x => x.Id == request.ScheduleId);
        //        if (seat == null)
        //        {
        //            return;
        //        }

        //        Ticket ticket = new Ticket();
        //        ticket.Code = request.Code;
        //        ticket.ScheduleId = request.ScheduleId;
        //        ticket.SeatId = seatId;
        //        ticket.PriceTicket = request.PriceTicket;
        //        ticket.IsActive = true;
        //        if (request.AddBillTickets != null)
        //        {
        //            AddBillTicketList(ticket.Id, request.AddBillTickets);
        //            _context.SaveChanges();
        //        }
        //        _context.tickets.Add(ticket);
        //    }
        //}

        //private void AddScheduleList(int roomId, List<Request_AddSchedule> requests)
        //{
        //    var room = _context.rooms.FirstOrDefault(x => x.Id == roomId);
        //    if (room == null)
        //    {
        //        return;
        //    }

        //    foreach (var request in requests)
        //    {
        //        var movie = _context.movies.FirstOrDefault(x => x.Id == request.MovieId);
        //        if (movie == null)
        //        {
        //            return;
        //        }

        //        Schedule schedule = new Schedule();
        //        schedule.Price = request.Price;
        //        schedule.StartAt = request.StartAt;
        //        schedule.EndAt = request.EndAt;
        //        schedule.Code = request.Code;
        //        schedule.MovieId = request.MovieId;
        //        schedule.Name = request.Name;
        //        schedule.IsActive = true;
        //        schedule.RoomId = roomId;
        //        if (request.AddTickets != null)
        //        {
        //            AddTicketList(schedule.Id, request.AddTickets);
        //            _context.SaveChanges();
        //        }
        //        _context.schedules.Add(schedule);
        //    }
        //}

        //public void AddTicketList(int scheduleId, List<Request_AddTicket> requests)
        //{
        //    var schedule = _context.schedules.FirstOrDefault(x => x.Id == scheduleId);
        //    if (schedule == null)
        //    {
        //        return;
        //    }

        //    foreach (var request in requests)
        //    {
        //        var seat = _context.seats.FirstOrDefault(x => x.Id == request.SeatId);
        //        if (seat == null)
        //        {
        //            return;
        //        }

        //        Ticket ticket = new Ticket();
        //        ticket.Code = request.Code;
        //        ticket.ScheduleId = scheduleId;
        //        ticket.SeatId = request.SeatId;
        //        ticket.PriceTicket = request.PriceTicket;
        //        ticket.IsActive = true;
        //        if (request.AddBillTickets != null)
        //        {
        //            AddBillTicketList(ticket.Id, request.AddBillTickets);
        //            _context.SaveChanges();
        //        }
        //        _context.tickets.Add(ticket);
        //    }
        //}

        //public void AddBillTicketList(int ticketId, List<Request_AddBillTicket> requests)
        //{
        //    var ticket = _context.tickets.FirstOrDefault(x => x.Id == ticketId);
        //    if (ticket == null)
        //    {
        //        return;
        //    }

        //    foreach (var request in requests)
        //    {
        //        var bill = _context.bills.FirstOrDefault(x => x.Id == request.BillId);
        //        if (bill == null)
        //        {
        //            return;
        //        }

        //        BillTicket billTicket = new BillTicket();
        //        billTicket.Quantity = request.Quantity;
        //        billTicket.TicketId = ticketId;
        //        billTicket.BillId = request.BillId;
        //        _context.billTickets.Add(billTicket);
        //    }
        //}
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
            return list;
        }

        //private List<Seat> EditSeatList(int roomId, List<Request_EditSeat> requests) {
        //    var room = _context.rooms.FirstOrDefault(x => x.Id == roomId);
        //    if (room == null)
        //    {
        //        return null;
        //    }
        //    List<Seat> list = new List<Seat>();
        //    foreach (var request in requests)
        //    {
        //        var seatStatus = _context.seatStatus.SingleOrDefault(x => x.Id == request.SeatStatusId);
        //        var seatType = _context.seatTypes.SingleOrDefault(x => x.Id == request.SeatTypeId);
        //        if (seatStatus == null || seatType == null)
        //        {
        //            return null;
        //        }

        //        Seat seat = new Seat();
        //        seat.Number = request.Number;
        //        seat.SeatStatusId = request.SeatStatusId;
        //        seat.Line = request.Line;
        //        seat.RoomId = request.RoomId;
        //        seat.SeatTypeId = request.SeatTypeId;
        //        seat.IsActive = true;

        //        if (request.EditTickets != null)
        //        {
        //            seat.Tickets = EditTicketListWithSeat(seat.Id, request.EditTickets);
        //            _context.SaveChanges();
        //        }
        //        list.Add(seat);
        //    }
        //    return list;
        //}

        //private List<Ticket> EditTicketListWithSeat(int seatId, List<Request_EditTicket> requests)
        //{
        //    var seat = _context.seats.FirstOrDefault(x => x.Id == seatId);
        //    if (seat == null)
        //    {
        //        return null;
        //    }

        //    List<Ticket> list = new List<Ticket>();
        //    foreach (var request in requests)
        //    {
        //        var schedule = _context.schedules.FirstOrDefault(x => x.Id == request.ScheduleId);
        //        if (schedule == null)
        //        {
        //            return null;
        //        }

        //        Ticket ticket = new Ticket();
        //        ticket.Code = request.Code;
        //        ticket.ScheduleId = request.ScheduleId;
        //        ticket.SeatId = seatId;
        //        ticket.PriceTicket = request.PriceTicket;
        //        ticket.IsActive = true;
        //        if (request.EditBillTickets != null)
        //        {
        //            ticket.BillTickets = EditBillTicketList(ticket.Id, request.EditBillTickets);
        //        }
        //        list.Add(ticket);
        //    }
        //    return list;
        //}

        //private List<Schedule> EditScheduleList(int roomId, List<Request_EditSchedule> requests)
        //{
        //    var room = _context.rooms.FirstOrDefault(x => x.Id == roomId);
        //    if (room == null)
        //    {
        //        return null;
        //    }

        //    List<Schedule> list = new List<Schedule>();
        //    foreach (var request in requests)
        //    {
        //        var movie = _context.movies.FirstOrDefault(x => x.Id == request.MovieId);
        //        if (movie == null)
        //        {
        //            return null;
        //        }

        //        Schedule schedule = new Schedule();
        //        schedule.Price = request.Price;
        //        schedule.StartAt = request.StartAt;
        //        schedule.EndAt = request.EndAt;
        //        schedule.Code = request.Code;
        //        schedule.MovieId = request.MovieId;
        //        schedule.Name = request.Name;
        //        schedule.RoomId = roomId;
        //        schedule.IsActive = true;
        //        if (request.EditTickets != null)
        //        {
        //            schedule.Tickets = EditTicketList(schedule.Id, request.EditTickets);
        //        }
        //        list.Add(schedule);
        //    }
        //    return list;
        //}

        //private List<Ticket> EditTicketList(int scheduleId, List<Request_EditTicket> requests)
        //{
        //    var schedule = _context.schedules.FirstOrDefault(x => x.Id == scheduleId);
        //    if (schedule == null)
        //    {
        //        return null;
        //    }

        //    List<Ticket> list = new List<Ticket>();
        //    foreach (var request in requests)
        //    {
        //        var seat = _context.seats.FirstOrDefault(x => x.Id == request.SeatId);
        //        if (seat == null)
        //        {
        //            return null;
        //        }

        //        Ticket ticket = new Ticket();
        //        ticket.Code = request.Code;
        //        ticket.ScheduleId = scheduleId;
        //        ticket.SeatId = request.SeatId;
        //        ticket.PriceTicket = request.PriceTicket;
        //        ticket.IsActive = true;
        //        if (request.EditBillTickets != null)
        //        {
        //            ticket.BillTickets = EditBillTicketList(ticket.Id, request.EditBillTickets);
        //        }
        //        list.Add(ticket);
        //    }
        //    return list;
        //}

        //private List<BillTicket> EditBillTicketList(int ticketId, List<Request_EditBillTicket> requests)
        //{
        //    var ticket = _context.tickets.FirstOrDefault(x => x.Id == ticketId);
        //    if (ticket == null)
        //    {
        //        return null;
        //    }

        //    List<BillTicket> list = new List<BillTicket>();
        //    foreach (var request in requests)
        //    {
        //        var bill = _context.bills.FirstOrDefault(x => x.Id == request.BillId);
        //        if (bill == null)
        //        {
        //            return null;
        //        }

        //        BillTicket billTicket = new BillTicket();
        //        billTicket.Quantity = request.Quantity;
        //        billTicket.TicketId = ticketId;
        //        billTicket.BillId = request.BillId;
        //        list.Add(billTicket);
        //    }
        //    return list;
        //}

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

            if(existingCinema.Rooms != null)
            {
                _context.rooms.RemoveRange(existingCinema.Rooms);
            }

            _context.cinemas.Remove(existingCinema);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The movie has been deleted successfully!", null);
        }

    }
}
