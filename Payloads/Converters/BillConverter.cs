using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class BillConverter
    {
        private readonly AppDbContext _context;
        private readonly BillTicketConverter _billTicketConverter;

        public BillConverter(AppDbContext context, BillTicketConverter billTicketConverter)
        {
            _context = context;
            _billTicketConverter = billTicketConverter;
        }

        public DataResponseBill EntityToDTO(Bill bill)
        {
            var billTickets = _context.billTickets
                                .Include(bt => bt.Ticket)
                                .ThenInclude(t => t.Schedule)
                                //.ThenInclude(s => s.Movie)
                                .ThenInclude(m => m.Room)
                                .ThenInclude(r => r.Cinema)
                                .Where(bt => bt.BillId == bill.Id)
                                .ToList();

            return new DataResponseBill()
            {
                TotalMoney = bill.TotalMoney,
                TradingCode = bill.TradingCode,
                CreateTime = bill.CreateTime,
                CustomerName = _context.users.SingleOrDefault(x => x.Id == bill.CustomerId).Name,
                Name = bill.Name,
                PromotionPercent = _context.promotions.SingleOrDefault(x => x.Id == bill.PromotionId).Percent,
                BillStatusName = _context.billStatus.SingleOrDefault(x => x.Id == bill.BillStatusId).Name,
                DataResponseBillFoods = _context.billFoods
                                            .Where(x => x.BillId == bill.Id)
                                            .Select(billFood => new DataResponseBillFood()
                                            {
                                                Quantity = billFood.Quantity,
                                                FoodName = _context.foods.SingleOrDefault(x => x.Id == billFood.FoodId).NameOfFood
                                            }),
                DataResponseBillTickets = _context.billTickets
                                            .Where(x => x.BillId == bill.Id)
                                            .Select(x => _billTicketConverter.EntityToDTO(x)),
            };
        }
    }
}
