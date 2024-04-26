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
    public class BillService : IBillService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseBill> _responseObject;
        private readonly ResponseObject<DataResponseAllBill> _responseObjectBill;
        private readonly BillConverter _converter;

        public BillService(AppDbContext context, ResponseObject<DataResponseBill> responseObject, BillConverter converter, ResponseObject<DataResponseAllBill> responseObjectBill)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
            _responseObjectBill = responseObjectBill;
        }

        public ResponseObject<DataResponseBill> AddBill(Request_AddBill request)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }
            var customer = _context.users.SingleOrDefault(x => x.Id == request.CustomerId);
            var promotion = _context.promotions.SingleOrDefault(x => x.Id == request.PromotionId);
            var billStatus = _context.billStatus.SingleOrDefault(x => x.Id == request.BillStatusId);

            if (customer == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The customer doesn't exist", null);
            }

            if (promotion == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The promotion doesn't exist", null);
            }

            if (billStatus == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The bill status doesn't exist", null);
            }

            Bill bill = new Bill();
            bill.TotalMoney = 0;
            bill.TradingCode = request.TradingCode;
            bill.CreateTime = DateTime.Now;
            bill.CustomerId = request.CustomerId;
            bill.Name = request.Name;
            bill.UpdateTime = DateTime.Now;
            bill.PromotionId = request.PromotionId;
            bill.BillStatusId = request.BillStatusId;
            bill.IsActive = true;
            _context.bills.Add(bill);
            _context.SaveChanges();

            if(request.AddBillFoods != null)
            {
                bill.BillFoods = AddBillFoodList(bill.Id, request.AddBillFoods);
                _context.bills.Update(bill);
                _context.SaveChanges();
            }

            if(request.AddBillTickets  != null)
            {
                bill.BillTickets = AddBillTicketList(bill.Id, request.AddBillTickets);
                _context.bills.Update(bill);
                _context.SaveChanges();
            }

            // Tính tổng số tiền của hóa đơn trước khi áp dụng khuyến mãi
            double totalMoneyBeforeDiscount = CalculateTotalMoney(request);

            // Áp dụng khuyến mãi và tính toán tổng số tiền cuối cùng
            double totalMoneyAfterDiscount = ApplyPromotions(totalMoneyBeforeDiscount, request.PromotionId);

            // Cập nhật giá trị TotalMoney của hóa đơn
            bill.TotalMoney = totalMoneyAfterDiscount;
            _context.bills.Update(bill);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Add bill successfully!", _converter.EntityToDTO(bill));
        }

        private double CalculateTotalMoney(Request_AddBill request)
        {
            // Tính tổng số tiền của hóa đơn trước khi áp dụng khuyến mãi
            double totalMoney = 0;

            // Tính tổng số tiền của các món ăn
            if (request.AddBillFoods != null)
            {
                foreach (var item in request.AddBillFoods)
                {
                    var food = _context.foods.SingleOrDefault(x => x.Id == item.FoodId);
                    if (food != null)
                    {
                        totalMoney += food.Price * item.Quantity;
                    }
                }
            }

            // Tính tổng số tiền của các vé
            if (request.AddBillTickets != null)
            {
                foreach (var item in request.AddBillTickets)
                {
                    var ticket = _context.tickets.SingleOrDefault(x => x.Id == item.TicketId);
                    if (ticket != null)
                    {
                        totalMoney += ticket.PriceTicket * item.Quantity;
                    }
                }
            }

            return totalMoney;
        }

        private double ApplyPromotions(double totalMoneyBeforeDiscount, int promotionId)
        {
            // Truy vấn thông tin về khuyến mãi từ bảng Promotion
            var promotion = _context.promotions.SingleOrDefault(x => x.Id == promotionId);
            if (promotion == null)
            {
                // Nếu không tìm thấy thông tin về khuyến mãi, trả về tổng số tiền trước khi áp dụng khuyến mãi
                return totalMoneyBeforeDiscount;
            }
            // Kiểm tra thời gian hiệu lực của mã giảm giá
            if (promotion.StartTime <= DateTime.Now && DateTime.Now <= promotion.EndTime && promotion.IsActive && promotion.Quantity > 0)
            {
                // Áp dụng khuyến mãi vào tổng số tiền
                double totalMoneyAfterDiscount = totalMoneyBeforeDiscount;

                if (promotion.Type == "Percent")
                {
                    // Nếu loại khuyến mãi là giảm giá theo tỷ lệ (%)
                    totalMoneyAfterDiscount *= (100 - promotion.Percent) / 100.0;
                }
                else if (promotion.Type == "FixedAmount")
                {
                    // Nếu loại khuyến mãi là giảm giá cố định
                    totalMoneyAfterDiscount -= promotion.Quantity;
                }
                // Các loại khuyến mãi khác có thể được xử lý tương tự ở đây

                return totalMoneyAfterDiscount;
            }
            else
            {
                // Nếu mã giảm giá không còn hiệu lực, trả về tổng số tiền trước khi áp dụng khuyến mãi
                return totalMoneyBeforeDiscount;
            }
        }

        private List<BillFood> AddBillFoodList(int billId, List<Request_BillFoodOfBill> requests)
        {
            var bill = _context.bills.FirstOrDefault(x => x.Id == billId);  
            if (bill == null)
            {
                return null;
            }

            List<BillFood> list = new List<BillFood>();
            foreach (var request in requests)
            {
                var food = _context.foods.SingleOrDefault(x => x.Id == request.FoodId);
                if(food == null)
                {
                    return null;
                }

                BillFood billFood = new BillFood();
                billFood.Quantity = request.Quantity;
                billFood.BillId = billId;
                billFood.FoodId = request.FoodId;
                list.Add(billFood);
            }
            _context.billFoods.AddRange(list);
            _context.SaveChanges();
            return list;
        }

        private List<BillTicket>AddBillTicketList(int billId, List<Request_BillTicketOfBill> requests)
        {
            var bill = _context.bills.FirstOrDefault(x => x.Id == billId);
            if (bill == null)
            {
                return null;
            }

            List<BillTicket> list = new List<BillTicket>();
            foreach (var request in requests)
            {
                var ticket = _context.tickets.FirstOrDefault(x => x.Id == request.TicketId);
                if(ticket == null)
                {
                    return null;
                }

                BillTicket billTicket = new BillTicket();
                billTicket.Quantity = request.Quantity;
                billTicket.BillId = billId;
                billTicket.TicketId = request.TicketId;
                list.Add(billTicket);
            }

            _context.billTickets.AddRange(list);
            _context.SaveChanges();
            return list;
        }

        public ResponseObject<DataResponseBill> EditBill(Request_EditBill request, int id)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }
            var bill = _context.bills.SingleOrDefault(x => x.Id == id);
            var customer = _context.users.SingleOrDefault(x => x.Id == request.CustomerId);
            var promotion = _context.promotions.SingleOrDefault(x => x.Id == request.PromotionId);
            var billStatus = _context.billStatus.SingleOrDefault(x => x.Id == request.BillStatusId);

            if (bill == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The bill doesn't exist", null);
            }

            if (customer == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The customer doesn't exist", null);
            }

            if (promotion == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The promotion doesn't exist", null);
            }

            if (billStatus == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The bill status doesn't exist", null);
            }

            // Kiểm tra xem mã giảm giá còn hiệu lực và isActive hay không
            if (promotion.IsActive && promotion.StartTime <= DateTime.Now && DateTime.Now <= promotion.EndTime)
            {
                bill.PromotionId = request.PromotionId; // Cập nhật mã giảm giá mới

                if (request.EditBillFoods != null)
                {
                    bill.BillFoods = EditBillFoodList(bill.Id, request.EditBillFoods);
                }

                if (request.EditBillTickets != null)
                {
                    bill.BillTickets = EditBillTicketList(bill.Id, request.EditBillTickets);
                }

                // Nếu có thay đổi về số lượng sản phẩm, cần tính toán lại tổng số tiền dựa trên các thông tin mới
                if (request.EditBillFoods != null || request.EditBillTickets != null)
                {
                    double totalMoneyBeforeDiscount = CalculateTotalBillAmount(bill);
                    double totalMoneyAfterDiscount = ApplyPromotions(totalMoneyBeforeDiscount, request.PromotionId);

                    // Cập nhật tổng số tiền mới vào hóa đơn
                    bill.TotalMoney = totalMoneyAfterDiscount;  
                }
            }
            else
            {
                // Nếu mã giảm giá không còn hiệu lực hoặc không isActive, áp dụng giá trị mặc định cho mã giảm giá của hóa đơn
                bill.PromotionId = 0;
            }

            bill.TradingCode = request.TradingCode;
            bill.CustomerId = request.CustomerId;
            bill.Name = request.Name;
            bill.UpdateTime = DateTime.Now;
            bill.BillStatusId = request.BillStatusId;

            _context.bills.Update(bill);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit bill successfully!", _converter.EntityToDTO(bill));
        }

        // Tính tổng số tiền của hóa đơn sau khi chỉnh sửa
        private double CalculateTotalBillAmount(Bill bill)
        {
            double totalMoney = 0;

            // Tính tổng số tiền của các món ăn
            foreach (var item in bill.BillFoods)
            {
                var food = _context.foods.SingleOrDefault(x => x.Id == item.FoodId);
                if (food != null)
                {
                    totalMoney += food.Price * item.Quantity;
                }
            }

            // Tính tổng số tiền của các vé
            foreach (var item in bill.BillTickets)
            {
                var ticket = _context.tickets.SingleOrDefault(x => x.Id == item.TicketId);
                if (ticket != null)
                {
                    totalMoney += ticket.PriceTicket * item.Quantity;
                }
            }

            return totalMoney;
        }

        private List<BillFood> EditBillFoodList(int billId, List<Request_BillFoodOfBill> requests)
        {
            var bill = _context.bills.FirstOrDefault(x => x.Id == billId);
            if (bill == null)
            {
                return null;
            }

            List<BillFood> list = new List<BillFood>();
            foreach (var request in requests)
            {
                var food = _context.foods.SingleOrDefault(x => x.Id == request.FoodId);
                if (food == null)
                {
                    return null;
                }

                BillFood billFood = new BillFood();
                billFood.Quantity = request.Quantity;
                billFood.BillId = billId;
                billFood.FoodId = request.FoodId;
                list.Add(billFood);
            }
            _context.billFoods.UpdateRange(list);
            _context.SaveChanges();
            return list;
        }

        private List<BillTicket> EditBillTicketList(int billId, List<Request_BillTicketOfBill> requests)
        {
            var bill = _context.bills.FirstOrDefault(x => x.Id == billId);
            if (bill == null)
            {
                return null;
            }

            List<BillTicket> list = new List<BillTicket>();
            foreach (var request in requests)
            {
                var ticket = _context.tickets.FirstOrDefault(x => x.Id == request.TicketId);
                if (ticket == null)
                {
                    return null;
                }

                BillTicket billTicket = new BillTicket();
                billTicket.Quantity = request.Quantity;
                billTicket.BillId = billId;
                billTicket.TicketId = request.TicketId;
                list.Add(billTicket);
            }

            _context.billTickets.UpdateRange(list);
            _context.SaveChanges();
            return list;
        }

        public ResponseObject<DataResponseBill> DeleteBill(int id)
        {
            var existingBill = _context.bills
                                    .Include(bill => bill.BillFoods)
                                    .Include(bill => bill.BillTickets)
                                    .SingleOrDefault(bill => bill.Id == id);

            if(existingBill == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The bill is not found. Please check again!", null);
            }

            existingBill.IsActive = false;

            _context.bills.Update(existingBill);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The bill has been deleted successfully!", null);
        }

        public ResponseObject<DataResponseAllBill> GetBill(int id)
        {
            var bill = _context.bills
                .Where(b => b.Id == id)
                .Include(b => b.BillTickets)
                .Include(b => b.BillStatus)
                .Include(b => b.Customer)
                .Include(b => b.Promotion)
                .Include(b => b.BillFoods)
                .Select(b => new DataResponseAllBill
                {
                    TotalMoney = b.TotalMoney,
                    CreateTime = b.CreateTime,
                    MovieName = b.BillTickets.FirstOrDefault().Ticket.Schedule.Movie.Name,
                    SeatNumber = b.BillTickets.FirstOrDefault().Ticket.Seat.Number,
                    SeatLine = b.BillTickets.FirstOrDefault().Ticket.Seat.Line,
                    RoomName = b.BillTickets.FirstOrDefault().Ticket.Schedule.Room.Name,
                    StartAt = b.BillTickets.FirstOrDefault().Ticket.Schedule.StartAt,
                    EndAt = b.BillTickets.FirstOrDefault().Ticket.Schedule.EndAt,
                    ScheduleName = b.BillTickets.FirstOrDefault().Ticket.Schedule.Name,
                    CinemaName = b.BillTickets.FirstOrDefault().Ticket.Schedule.Room.Cinema.NameOfCinema,
                    CinemaAddress = b.BillTickets.FirstOrDefault().Ticket.Schedule.Room.Cinema.Address,
                    TicketCode = b.BillTickets.FirstOrDefault().Ticket.Code
                })
                .SingleOrDefault();

            return _responseObjectBill.ResponseSuccess("Retrieve bill data successfully", bill);
        }

    }
}


