using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.Entities;

namespace MovieManagementSystem.DataContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<ConfirmEmail> confirmEmails { get; set; }
        public DbSet<RankCustomer> rankCustomers { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }
        public DbSet<UserStatus> userStatus { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($"Server = LAPTOP-TIEN; Database = MovieManagementSystem; Trusted_Connection = True; TrustServerCertificate=True");
        }
    }
}
