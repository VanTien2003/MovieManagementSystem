using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IVnPayService
    {
        Task<ResponseObject<string>> CreatePaymentUrl(int billId, HttpContext context, int userId);
        Task<ResponseObject<DataResponseVnPayment>> PaymentExecute(IQueryCollection vnpayData);
    }
}
