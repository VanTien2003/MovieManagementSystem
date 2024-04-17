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
    public class ScheduleService : IScheduleService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseSchedule> _responseObject;
        private readonly ScheduleConverter _converter;

        public ScheduleService(AppDbContext context, ResponseObject<DataResponseSchedule> responseObject, ScheduleConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponseSchedule> AddSchedule(Request_AddSchedule request)
        {
            var movie = _context.movies.SingleOrDefault(s => s.Id == request.MovieId);
            var room = _context.rooms.SingleOrDefault(x => x.Id == request.RoomId);
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }
            if (movie == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The movie status doesn't exist", null);
            }
            if (room == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The room doesn't exist", null);
            }

            Schedule schedule = new Schedule()
            {
                Price = request.Price,
                StartAt = request.StartAt,
                EndAt = request.EndAt,
                Code = request.Code,
                MovieId = request.MovieId,
                Name = request.Name,
                RoomId = request.RoomId,
                IsActive = true
            };
            _context.schedules.Add(schedule);
            _context.SaveChanges();

            if(request.AddTickets != null)
            {
                schedule.Tickets = AddTicketList(schedule.Id, request.AddTickets);
                _context.schedules.Update(schedule);
                _context.SaveChanges();
            }
            return _responseObject.ResponseSuccess("Add schedule successfully!", _converter.EntityToDTO(schedule));
        }

        public List<Ticket> AddTicketList(int scheduleId, List<Request_TicketOfSchedule> requests)
        {           
            var schedule = _context.schedules.FirstOrDefault(x => x.Id == scheduleId);
            if (schedule == null)
            {
                return null;
            }

            List<Ticket> list = new List<Ticket>();
            foreach (var request in requests)
            {
                var seat = _context.seats.FirstOrDefault(x => x.Id == request.SeatId);
                if (seat == null)
                {
                    return null;
                }

                Ticket ticket = new Ticket();
                ticket.Code = request.Code;
                ticket.ScheduleId = scheduleId;
                ticket.SeatId = request.SeatId;
                ticket.PriceTicket = request.PriceTicket;
                ticket.IsActive = true;
                list.Add(ticket);
            }
            _context.tickets.AddRange(list);
            _context.SaveChanges();
            return list;
        }


        public ResponseObject<DataResponseSchedule> EditSchedule(Request_EditSchedule request, int id)
        {
            var schedule = _context.schedules.SingleOrDefault(x => x.Id == id);
            var movie = _context.movies.SingleOrDefault(s => s.Id == request.MovieId);
            var room = _context.rooms.SingleOrDefault(x => x.Id == request.RoomId);
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }
            if (movie == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The movie status doesn't exist", null);
            }
            if (room == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The room doesn't exist", null);
            }
            if (schedule == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The schedule doesn't exist", null);
            }

            schedule.Price = request.Price;
            schedule.StartAt = request.StartAt;
            schedule.EndAt = request.EndAt;
            schedule.MovieId = request.MovieId;
            schedule.RoomId = request.RoomId;
            schedule.Code = request.Code;
            schedule.Name = request.Name;

            if (request.EditTickets != null)
            {
                schedule.Tickets = EditTicketList(schedule.Id, request.EditTickets);
            }
            _context.schedules.Update(schedule);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit schedule successfully!", _converter.EntityToDTO(schedule));
        }

        private List<Ticket> EditTicketList(int scheduleId, List<Request_TicketOfSchedule> requests)
        {
            var schedule = _context.schedules.FirstOrDefault(x => x.Id == scheduleId);
            if (schedule == null)
            {
                return null;
            }

            List<Ticket> list = new List<Ticket>();
            foreach (var request in requests)
            {
                var seat = _context.seats.FirstOrDefault(x => x.Id == request.SeatId);
                if (seat == null)
                {
                    return null;
                }

                Ticket ticket = new Ticket();
                ticket.Code = request.Code;
                ticket.ScheduleId = scheduleId;
                ticket.SeatId = request.SeatId;
                ticket.PriceTicket = request.PriceTicket;
                ticket.IsActive = true;
                list.Add(ticket);
            }

            return list;
        }
        public ResponseObject<DataResponseSchedule> DeleteSchedule(int id)
        {
            var existingSchedule = _context.schedules
                                        .Where(schedule => schedule.Id == id)
                                        .Include(schedule => schedule.Tickets)
                                        .SingleOrDefault();

            if(existingSchedule == null) {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The schedule is not found. Please check again!", null);
            }

            if(existingSchedule.Tickets != null)
            {
                _context.tickets.RemoveRange(existingSchedule.Tickets);
            }

            _context.schedules.Remove(existingSchedule);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The schedule has been deleted successfully!", null);
        }
    }
}
