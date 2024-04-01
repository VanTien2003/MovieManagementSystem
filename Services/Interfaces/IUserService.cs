using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        ResponseObject<DataResponseUser> Register(Request_Register request);
        DataResponseToken GenerateAccessToken(User user);
        DataResponseToken RenewAccessToken(Request_RenewAccessToken request);
        ResponseObject<DataResponseToken> Login(Request_Login request);
        IQueryable<DataResponseUser> GetAll();
    }
}
