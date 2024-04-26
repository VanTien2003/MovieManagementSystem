using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.Entities;

namespace MovieManagementSystem.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<ConfirmEmail> confirmEmails { get; set; }
        public DbSet<RankCustomer> rankCustomers { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }
        public DbSet<UserStatus> userStatus { get; set; }
        public DbSet<Bill> bills { get; set; }
        public DbSet<BillFood> billFoods { get; set; }
        public DbSet<BillStatus> billStatus { get; set; }  
        public DbSet<BillTicket> billTickets { get; set; }
        public DbSet<Ticket> tickets { get; set; }
        public DbSet<Banner> banners { get; set; }
        public DbSet<Cinema> cinemas { get; set; }
        public DbSet<Food> foods { get; set; }
        public DbSet<GeneralSetting> generalSettings { get; set; }
        public DbSet<Movie> movies { get; set; }
        public DbSet<MovieType> movieTypes { get; set; }
        public DbSet<Promotion> promotions { get; set; }
        public DbSet<Rate> rates { get; set; }
        public DbSet<Room> rooms { get; set; }
        public DbSet<Schedule> schedules { get; set; }
        public DbSet<Seat> seats { get; set; }
        public DbSet<SeatStatus> seatStatus { get; set; }
        public DbSet<SeatType> seatTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
