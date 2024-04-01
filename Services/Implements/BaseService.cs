using MovieManagementSystem.DataContext;

namespace MovieManagementSystem.Services.Implements
{
    public class BaseService
    {
        public readonly AppDbContext _context;
        public BaseService()
        {
            _context = new AppDbContext();
        }
    }
}
