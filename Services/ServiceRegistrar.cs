using MovieManagementSystem.Payloads.Converters;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;
using MovieManagementSystem.Services.Implements;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Services
{
    public class ServiceRegistrar
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Add scoped services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<ISeatService, SeatService>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<ICinemaService, CinemaService>();
            services.AddScoped<IScheduleService, ScheduleService>();

            // Add singleton response objects
            services.AddSingleton<ResponseObject<DataResponseUser>>();
            services.AddSingleton<ResponseObject<DataResponseToken>>();
            services.AddSingleton<ResponseObject<DataResponseMovie>>();
            services.AddSingleton<ResponseObject<DataResponseSchedule>>();
            services.AddSingleton<ResponseObject<DataResponseTicket>>();
            services.AddSingleton<ResponseObject<DataResponseFood>>();
            services.AddSingleton<ResponseObject<DataResponseBillFood>>();
            services.AddSingleton<ResponseObject<DataResponseCinema>>();
            services.AddSingleton<ResponseObject<DataResponseRoom>>();
            services.AddSingleton<ResponseObject<DataResponseBillTicket>>();
            services.AddSingleton<ResponseObject<DataResponseSeat>>();
            services.AddSingleton<ResponseObject<DataResponseRenewAccessToken>>();

            // Add scoped converters
            services.AddScoped<UserConverter>();
            services.AddScoped<MovieConverter>();
            services.AddScoped<ScheduleConverter>();
            services.AddScoped<TicketConverter>();
            services.AddScoped<BillTicketConverter>();
            services.AddScoped<FoodConverter>();
            services.AddScoped<BillFoodConverter>();
            services.AddScoped<RoomConverter>();
            services.AddScoped<SeatConverter>();
            services.AddScoped<CinemaConverter>();
            services.AddScoped<RoomOfCinemaConverter>();
            services.AddScoped<ScheduleOfMovieConverter>();
            services.AddScoped<ScheduleOfRoomConverter>();
            services.AddScoped<SeatOfRoomConverter>();
            services.AddScoped<TicketOfSeatConverter>();
            services.AddScoped<TicketOfScheduleConverter>();
        }
    }
}
