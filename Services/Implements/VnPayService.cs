using Azure;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Helpers;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;
using MovieManagementSystem.Services.Interfaces;
using System;
using System.Net.Mail;
using System.Text;

namespace MovieManagementSystem.Services.Implements
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseVnPayment> _responseObject;

        public VnPayService(IConfiguration config, AppDbContext context, ResponseObject<DataResponseVnPayment> responseObject)
        {
            _config = config;
            _context = context;
            _responseObject = responseObject;
        }

        public async Task<ResponseObject<string>> CreatePaymentUrl(int billId, HttpContext context, int userId)
        {
            var bill = await _context.bills.SingleOrDefaultAsync(x => x.Id == billId);
            var user = await _context.users.SingleOrDefaultAsync(x => x.Id == userId);
            if(bill == null)
            {
                return new ResponseObject<string>(StatusCodes.Status400BadRequest, "The bill doesn't exist. Please check again!", "");
            }

            if(user == null)
            {
                return new ResponseObject<string>(StatusCodes.Status400BadRequest, "The user doesn't exist. Please check again!", "");
            }
            if(user.Id == bill.CustomerId)
            {
                if(bill.BillStatusId == 1 && bill.TotalMoney != 0)
                {
                    var pay = new VnPayLibrary();
                    pay.AddRequestData("vnp_Version", _config["Vnpay:Version"]!);
                    pay.AddRequestData("vnp_Command", _config["Vnpay:Command"]!);
                    pay.AddRequestData("vnp_TmnCode", _config["Vnpay:TmnCode"]!);
                    pay.AddRequestData("vnp_Amount", ((int)bill.TotalMoney * 100).ToString());
                    pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    pay.AddRequestData("vnp_CurrCode", _config["Vnpay:CurrCode"]!);
                    pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
                    pay.AddRequestData("vnp_Locale", _config["Vnpay:Locale"]!);
                    pay.AddRequestData("vnp_OrderInfo", $"Thanh toán hóa đơn {billId}");
                    pay.AddRequestData("vnp_OrderType", "other");
                    pay.AddRequestData("vnp_ReturnUrl", _config["PaymentCallBack:ReturnUrl"]!);
                    pay.AddRequestData("vnp_TxnRef", billId.ToString());

                    var paymentUrl = pay.CreateRequestUrl(_config["Vnpay:BaseUrl"], _config["Vnpay:HashSecret"]);
                    return new ResponseObject<string>(StatusCodes.Status200OK, "Add payment url successfully!", paymentUrl);
                }else
                {
                    return new ResponseObject<string>(StatusCodes.Status400BadRequest, "The bill doesn't valid. Please check your bill again!", "");
                }
            }
            return new ResponseObject<string>(StatusCodes.Status400BadRequest, "Error! An error occurred. Please check again later!", "");
        }

        public async Task<ResponseObject<DataResponseVnPayment>> PaymentExecute(IQueryCollection vnpayData)
        {
            string vnp_TmnCode = _config["Vnpay:TmnCode"]!;
            string vnp_HashSecret = _config["Vnpay:HashSecret"]!;
            VnPayLibrary vnPay = new VnPayLibrary();

            foreach (var (key, value) in vnpayData)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnPay.AddResponseData(key, value);
                }
            }

            string billId = vnPay.GetResponseData("vnp_TxnRef");
            string vnp_ResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnPay.GetResponseData("vnp_TransactionStatus");
            string vnp_SecureHash = vnPay.GetResponseData("vnp_SecureHash");
            double vnp_Amount = Convert.ToDouble(vnPay.GetResponseData("vnp_Amount"));
            bool check = vnPay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            if (check)
            {
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    var bill = await _context.bills.Include(x => x.BillTickets)
                                                   .ThenInclude(x => x.Ticket)
                                                   .ThenInclude(x => x.Seat)
                                                   .FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(billId));

                    if (bill == null)
                    {
                        return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The bill not found", new DataResponseVnPayment
                        {
                            Success = false
                        });
                    }

                    bill.BillStatusId = 2;
                    var promotion = _context.promotions.SingleOrDefault(x => x.Id == bill.PromotionId);
                    if(promotion !=  null && promotion.Quantity > 0)
                    {
                        promotion.Quantity -= 1;
                        _context.promotions.Update(promotion);
                    }
                    bill.UpdateTime = DateTime.Now;

                    var billTicket = bill.BillTickets.Where(x => x.BillId == bill.Id).ToList();
                    foreach (var item in billTicket)
                    {
                        item.Ticket.IsActive = false;
                        item.Ticket.Seat.SeatStatusId = 2;
                        _context.seats.Update(item.Ticket.Seat);
                    }

                    _context.bills.Update(bill);
                    await _context.SaveChangesAsync();

                    var user = _context.users.FirstOrDefault(x => x.Id == bill.CustomerId);
                    if (user != null)
                    {
                        string toEmail = user.Email;
                        string subject = "Xác nhận hóa đơn thanh toán VNPAY";
                        string body =
                            $"Kính gửi," +
                            $"\r\n\r\nChúng tôi xin thông báo rằng thanh toán của bạn đã được xác nhận thành công." +
                            $"\r\n\r\nDưới đây là các chi tiết về giao dịch:" +
                            $"\r\n\r\n- Mã Giao Dịch: {billId}" +
                            $"\r\n- Ngày Giờ Thanh Toán: {bill.CreateTime.ToString("dd/MM/yyyy HH:mm:ss")}" +
                            $"\r\n- Mô Tả Hóa Đơn: {bill.Name}" +
                            $"\r\n- Số Tiền Thanh Toán: {bill.TotalMoney.ToString("#,##0")}VND" +
                            $"\r\nVui lòng giữ lại thông tin này để tham khảo trong trường hợp cần thiết." +
                            $"\r\n\r\nXin cảm ơn đã sử dụng dịch vụ của chúng tôi." +
                            $"\r\n\r\nTrân trọng.";

                        EmailService emailService = new EmailService();
                        var isValid = emailService.SendEmail(toEmail, subject, body);
                        if (!isValid)
                        {
                            return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Sending email failed.", new DataResponseVnPayment
                            {
                                Success = false
                            });
                        }
                    }
                    return _responseObject.ResponseSuccess($"Giao dịch thành công đơn hàng {bill.Id}", new DataResponseVnPayment
                    {
                        Success = true,
                        PaymentMethod = "VnPay",
                        OrderDescription = vnPay.GetResponseData("vnp_OrderInfo"),
                        OrderId = billId.ToString(),
                        PaymentId = vnPay.GetResponseData("vnp_TransactionNo"),
                        TransactionId = vnPay.GetResponseData("vnp_TransactionNo"),
                        Token = vnpayData.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value!,
                        VnPayResponseCode = vnPay.GetResponseData("vnp_ResponseCode")
                    });
                }
                else
                {
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, $"Error while performing transaction. Error code: {vnp_ResponseCode}", new DataResponseVnPayment
                    {
                        Success = false
                    });
                }
            }
            else
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "There was an error during processing. Please check again!", new DataResponseVnPayment
                {
                    Success = false
                });
            }
        }
    }
}
