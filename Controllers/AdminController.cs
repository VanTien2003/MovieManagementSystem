using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MovieManagementSystem.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICinemaService _cinemaService;
        private readonly IRoomService _roomService;
        private readonly ISeatService _seatService;
        private readonly IFoodService _foodService;
        private readonly IUserService _userService;
        public AdminController(AppDbContext context, ICinemaService cinemaService, IRoomService roomService, ISeatService seatService, IFoodService foodService, IUserService userService)
        {
            _context = context;
            _cinemaService = cinemaService;
            _roomService = roomService;
            _seatService = seatService;
            _foodService = foodService;
            _userService = userService;
        }
        // CRUD Cinema
        [HttpPost]
        public IActionResult AddCinema(Request_AddCinema request)
        {
            return Ok(_cinemaService.AddCinema(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCinema(Request_EditCinema request, int id)
        {
            return Ok(_cinemaService.EditCinema(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCinema(int id)
        {
            return Ok(_cinemaService.DeleteCinema(id));
        }

        // CRUD Cinema
        [HttpPost]
        public IActionResult AddRoom(Request_AddRoom request)
        {
            return Ok(_roomService.AddRoom(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRoom(Request_EditRoom request, int id)
        {
            return Ok(_roomService.EditRoom(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            return Ok(_roomService.DeleteRoom(id));
        }

        //CRUD Seat
        [HttpPost]
        public IActionResult AddSeat(Request_Seat request)
        {
            return Ok(_seatService.AddSeat(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSeat(Request_Seat request, int id)
        {
            return Ok(_seatService.EditSeat(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSeat(int id)
        {
            return Ok(_seatService.DeleteSeat(id));
        }

        // CRUD Food
        [HttpPost]
        public IActionResult AddFood(Request_Food request)
        {
            return Ok(_foodService.AddFood(request));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFood(Request_Food request, int id)
        {
            return Ok(_foodService.EditFood(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFood(int id)
        {
            return Ok(_foodService.DeleteFood(id));
        }

        [HttpGet]
        public ActionResult<IEnumerable<DataResponseCinemaRevenue>> GetCinemaRevenue(DateTime startDate, DateTime endDate)
        {
            try
            {
                var cinemaRevenue = _context.cinemas
                    .Join(_context.rooms, cinema => cinema.Id, room => room.CinemaId, (cinema, room) => new { Cinema = cinema, Room = room })
                    .Join(_context.schedules, cr => cr.Room.Id, schedule => schedule.RoomId, (cr, schedule) => new { cr.Cinema, Schedule = schedule })
                    .Join(_context.tickets, crs => crs.Schedule.Id, ticket => ticket.ScheduleId, (crs, ticket) => new { crs.Cinema, Ticket = ticket })
                    .Join(_context.billTickets, crst => crst.Ticket.Id, billTicket => billTicket.TicketId, (crst, billTicket) => new { crst.Cinema, BillTicket = billTicket })
                    .Join(_context.bills.Where(bill => bill.CreateTime >= startDate && bill.CreateTime <= endDate && bill.IsActive), 
                        crstb => crstb.BillTicket.BillId, bill => bill.Id, (crstb, bill) => new { crstb.Cinema, Bill = bill })
                    .GroupBy(x => new { x.Cinema.Id, x.Cinema.NameOfCinema })
                    .Select(group => new DataResponseCinemaRevenue
                    {
                        CinemaName = group.Key.NameOfCinema,
                        TotalRevenue = group.Sum(x => x.Bill.TotalMoney)
                    })
                    .ToList();
                return Ok(cinemaRevenue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<object>> GetTopSellingFoods()
        {
            try
            {
                var sevenDaysAgo = DateTime.Now.AddDays(-7);
                var topSellingFoods = _context.billFoods
                                        .Include(bf => bf.Food)
                                        .Where(bf => bf.Bill.CreateTime >= sevenDaysAgo)
                                        .GroupBy(bf => bf.Food)
                                        .Select(g => new
                                        {
                                            FoodName = g.Key.NameOfFood,
                                            TotalQuantitySold = g.Sum(bf => bf.Quantity)
                                        })
                                        .OrderByDescending(g => g.TotalQuantitySold)
                                        .Take(10)
                                        .ToList();
                return Ok(topSellingFoods);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }         
        }

        [HttpGet]
        public IActionResult GetUserById(int id)
        {
            return Ok(_userService.GetUserById(id));
        }

        [HttpGet]
        public ActionResult GetAllUser()
        {
            return Ok(_userService.GetAll());
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(Request_EditUserByAdmin request, int id)
        {
            return Ok(_userService.EditUserByAdmin(request, id));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            return Ok(_userService.DeleteUser(id));
        }
    }
}
