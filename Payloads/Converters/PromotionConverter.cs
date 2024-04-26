using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class PromotionConverter
    {
        private readonly AppDbContext _context;
        public PromotionConverter(AppDbContext context) {
            _context = context;
        }
        public DataResponsePromotion EntityToDTO(Promotion promotion)
        {
            return new DataResponsePromotion()
            {
                Percent = promotion.Percent,
                Quantity = promotion.Quantity,
                Type = promotion.Type,
                StartTime = promotion.StartTime,
                EndTime = promotion.EndTime,
                Description = promotion.Description,
                Name = promotion.Name,
                IsActive = promotion.IsActive,
                RankCustomerName = _context.rankCustomers.SingleOrDefault(x => x.Id == promotion.RankCustomerId).Name
            };
        }
    }
}
