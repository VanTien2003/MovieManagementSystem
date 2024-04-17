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
        DataResponseRenewAccessToken RenewAccessToken(Request_RenewAccessToken request);
        ResponseObject<DataResponseToken> Login(Request_Login request);
        List<DataResponseUser> GetAll();
        ResponseObject<bool> ChangePassword(Request_ChangePassword request, int userId);
        ResponseObject<bool> ConfirmAccount(string confirmationCode);
        ResponseObject<bool> SendConfirmationCode(string email);
        ResponseObject<bool> ResetPassword(string resetCode, string newPassword);
    }
}
