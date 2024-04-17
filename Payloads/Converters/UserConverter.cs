using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class UserConverter
    {
        private readonly AppDbContext _context;
        public UserConverter(AppDbContext context)
        {
            _context = context;
        }
        public DataResponseUser EntityToDTO(User user)
        {
            return new DataResponseUser()
            {
                Point = user.Point,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                RankCustomerName = _context.rankCustomers.SingleOrDefault(x => x.Id == user.RankCustomerId).Name,
                UserStatusName = _context.userStatus.SingleOrDefault(x => x.Id == user.UserStatusId).Name,
                RoleName = _context.roles.SingleOrDefault(x => x.Id == user.RoleId).RoleName,
                IsActive = user.IsActive
            };
        }
    }
}
