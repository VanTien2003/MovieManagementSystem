using MovieManagementSystem.Services.Interfaces;
using System.Net.Mail;

namespace MovieManagementSystem.Services.Implements
{
    public class EmailService : IEmailService
    {
        public bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                // Tạo và gửi email
                var fromAddress = new MailAddress("tien.pv.2054@aptechlearning.edu.vn", "Phùng Văn Tiến");
                var toAddress = new MailAddress(toEmail);
                const string fromPassword = "koje cejh ydsk sewj";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // Thay đổi thành SMTP server của bạn
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                return true; // Gửi email thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                return false; // Gửi email thất bại
            }
        }
    }
}
