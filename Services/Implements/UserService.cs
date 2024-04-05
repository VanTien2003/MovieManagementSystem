using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Handle.Email;
using MovieManagementSystem.Payloads.Converters;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;
using MovieManagementSystem.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using BcryptNet = BCrypt.Net.BCrypt;

namespace MovieManagementSystem.Services.Implements
{
    public class UserService : BaseService , IUserService
    {
        private readonly ResponseObject<DataResponseUser> _responseObject;
        private readonly UserConverter _converter;
        private readonly IConfiguration _configuration;
        private readonly ResponseObject<DataResponseToken> _responseTokenObject;
        public UserService(IConfiguration configuration)
        {
            _converter = new UserConverter();
            _responseObject = new ResponseObject<DataResponseUser>();
            _configuration = configuration;
            _responseTokenObject = new ResponseObject<DataResponseToken>();
        }

        public ResponseObject<DataResponseUser> Register(Request_Register request)
        {
            if(string.IsNullOrWhiteSpace(request.PhoneNumber) 
                || string.IsNullOrWhiteSpace(request.UserName) 
                || string.IsNullOrWhiteSpace(request.Password)
                || string.IsNullOrWhiteSpace(request.Name) 
                || string.IsNullOrWhiteSpace(request.Email)
                )
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }
            if(_context.users.Any(x => x.Email.Equals(request.Email)))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Email đã tồn tại trên hệ thống", null);
            }
            if (_context.users.Any(x => x.UserName.Equals(request.UserName)))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Tên tài khoản đã tồn tại trên hệ thống", null);
            }
            if(!Validate.IsValidEmail(request.Email))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Định dạng Email không hợp lệ", null);
            }
            if(!_context.rankCustomers.Any(x => x.Id == request.RankCustomerId))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Hạng khách hàng không tồn tại", null);
            }
            if (!_context.userStatus.Any(x => x.Id == request.UserStatusId))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Trạng thái người dùng không tồn tại", null);
            }

            // Tạo mã xác nhận
            var confirmationCode = GenerateConfirmationCode();

            // Gửi mã xác nhận qua email
            if (SendConfirmationEmail(request.Email, confirmationCode))
            {
                // Lưu thông tin tài khoản vào cơ sở dữ liệu
                var user = new User
                {
                    Point = 0,
                    Email = request.Email,
                    UserName = request.UserName,
                    Password = BcryptNet.HashPassword(request.Password),
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    RankCustomerId = request.RankCustomerId,
                    UserStatusId = request.UserStatusId,
                    RoleId = 3,
                    IsActive = false, // Tài khoản chưa được kích hoạt
                };

                _context.users.Add(user);
                _context.SaveChanges();

                // Lưu thông tin mã xác nhận vào bảng ConfirmEmail
                var confirmEmail = new ConfirmEmail
                {
                    UserId = user.Id,
                    ExpiredTime = DateTime.Now.AddMinutes(10), // Thời gian hết hạn: 10 phút sau khi tạo
                    ConfirmCode = confirmationCode,
                    IsConfirm = false // Chưa được xác nhận
                };

                _context.confirmEmails.Add(confirmEmail);
                _context.SaveChanges();

                // Trả về thông báo thành công
                return _responseObject.ResponseSuccess("Một mã xác nhận đã được gửi đến email của bạn. Vui lòng kiểm tra email để hoàn thành quá trình đăng ký.", null);
            }
            else
            {
                // Gửi email thất bại
                return _responseObject.ResponseError(StatusCodes.Status500InternalServerError, "Đã có lỗi xảy ra khi gửi email xác nhận. Vui lòng thử lại sau.", null);
            }
        }

        private string GenerateConfirmationCode()
        {
            // Tạo một mã xác nhận ngẫu nhiên (ví dụ: chuỗi số hoặc ký tự)
            // Bạn có thể sử dụng thư viện mã hóa cung cấp các phương thức để tạo mã xác nhận an toàn hơn
            Random random = new Random();
            string randomDigits = "";
            for (int i = 0; i < 6; i++)
            {
                randomDigits += random.Next(10); // Sinh ra một số ngẫu nhiên từ 0 đến 9 và thêm vào chuỗi
            }
            return randomDigits;
        }

        private bool SendConfirmationEmail(string emailAddress, string confirmationCode)
        {
            try
            {
                // Tạo và gửi email
                var fromAddress = new MailAddress("tien.pv.2054@aptechlearning.edu.vn", "Văn Tiến");
                var toAddress = new MailAddress(emailAddress);
                const string fromPassword = "koje cejh ydsk sewj";
                const string subject = "Xác nhận tài khoản Email";
                string body = $"Mã xác nhận của bạn là: {confirmationCode}";

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
                Console.WriteLine("Lỗi gửi email: " + ex.Message);
                return false; // Gửi email thất bại
            }
        }

        public ResponseObject<bool> ConfirmAccount(string confirmationCode)
        {
            var confirmEmail = _context.confirmEmails
                .FirstOrDefault(c => c.ConfirmCode == confirmationCode);

            if (confirmEmail != null)
            {
                // Kiểm tra xem mã xác nhận còn hạn sử dụng hay không
                if (confirmEmail.ExpiredTime >= DateTime.Now)
                {
                    // Xác minh mã xác nhận và kích hoạt tài khoản
                    var user = _context.users.FirstOrDefault(u => u.Id == confirmEmail.UserId);
                    if (user != null)
                    {
                        user.IsActive = true;
                        _context.SaveChanges();

                        // Đánh dấu mã xác nhận đã được sử dụng
                        confirmEmail.IsConfirm = true;
                        _context.SaveChanges();

                        return new ResponseObject<bool>(StatusCodes.Status200OK, "Tài khoản được kích hoạt thành công!", true);
                    }
                    else
                    {
                        return new ResponseObject<bool>(StatusCodes.Status404NotFound, "Không tìm thấy người dùng tương ứng với mã xác nhận.", false);
                    }
                }
                else if(!confirmEmail.IsConfirm)
                {
                    return new ResponseObject<bool>(StatusCodes.Status404NotFound, "Tài khoản email chưa được xác minh!", false);
                }
                else
                {
                    // Mã xác nhận đã hết hạn
                    return new ResponseObject<bool>(StatusCodes.Status404NotFound, "Mã xác minh đã hết hạn. Vui lòng gửi lại mã xác minh!", false);
                }
            }

            return new ResponseObject<bool>(StatusCodes.Status404NotFound, "Xác minh tài khoản thất bại. Người dùng hoặc mã xác minh không hợp lệ!", false);
        }

        public DataResponseToken RenewAccessToken(Request_RenewAccessToken request)
        {
            throw new NotImplementedException();
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using(var item = RandomNumberGenerator.Create())
            {
                item.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public DataResponseToken GenerateAccessToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value);

            var role = _context.roles.SingleOrDefault(x => x.Id == user.RoleId);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("RoleId", role.Id.ToString()),
                    new Claim(ClaimTypes.Role, role?.Code ?? "")
                }),
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            RefreshToken rf = new RefreshToken()
            {
                Token = refreshToken,
                ExpiredTime = DateTime.Now.AddDays(1),
                UserId = user.Id,
            };

            _context.refreshTokens.Add(rf);
            _context.SaveChanges();

            DataResponseToken result = new DataResponseToken()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
            return result;
        }

        public ResponseObject<DataResponseToken> Login(Request_Login request)
        {
            var user = _context.users.SingleOrDefault(x => x.UserName.Equals(request.UserName));

            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }

            if (user == null)
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản không tồn tại", null);
            }

            bool checkPass = BcryptNet.Verify(request.Password, user.Password);
            if(!checkPass)
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Mật khẩu không chính xác", null);
            }

            if (!user.IsActive)
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản chưa được kích hoạt", null);
            }

            return _responseTokenObject.ResponseSuccess("Đăng nhập thành công", GenerateAccessToken(user));
        }

        public PageResult<DataResponseUser> GetAll(Pagination pagination)
        {
            try
            {
                var danhSachNguoiDung = _context.users.Where(u => u.IsActive).AsNoTracking().AsQueryable();

                var result = PageResult<DataResponseUser>.ToPageResult(pagination, danhSachNguoiDung.Select(x => _converter.EntityToDTO(x)).AsQueryable());
                pagination.TotalCount = danhSachNguoiDung.Count();
                return new PageResult<DataResponseUser>(pagination, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }


        public ResponseObject<bool> ChangePassword(Request_ChangePassword request, int userId)
        {
            // Tìm người dùng trong cơ sở dữ liệu
            var user = _context.users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return new ResponseObject<bool>(StatusCodes.Status404NotFound, "Không tìm thấy người dùng.", false);
            }

            // Kiểm tra mật khẩu hiện tại của người dùng
            if (!BcryptNet.Verify(request.CurrentPassword, user.Password))
            {
                return new ResponseObject<bool>(StatusCodes.Status400BadRequest, "Mật khẩu hiện tại không chính xác.", false);
            }

            // Kiểm tra mật khẩu mới và mật khẩu xác nhận
            if (request.NewPassword != request.ConfirmNewPassword)
            {
                // Mật khẩu mới và mật khẩu xác nhận không khớp
                return new ResponseObject<bool>(StatusCodes.Status400BadRequest, "Mật khẩu mới và mật khẩu xác nhận không khớp.", false);
            }

            // Update password
            user.Password = BcryptNet.HashPassword(request.NewPassword);
            _context.SaveChanges();

            return new ResponseObject<bool>(StatusCodes.Status200OK, "Đổi mật khẩu thành công!", true);
        }

        public ResponseObject<bool> SendConfirmationCode(string email)
        {
            // Tìm người dùng trong cơ sở dữ liệu bằng email
            var user = _context.users.FirstOrDefault(u => u.Email.Equals(email));

            if (user == null)
            {
                // Không tìm thấy người dùng với địa chỉ email đã cung cấp
                return new ResponseObject<bool>(StatusCodes.Status404NotFound, "Không tìm thấy người dùng với địa chỉ email đã cung cấp.", false);
            }

            // Kiểm tra xem có mã xác nhận nào hết hạn không
            var expiredConfirmEmails = _context.confirmEmails.Where(c => c.UserId == user.Id && c.ExpiredTime < DateTime.Now).ToList();
            if (expiredConfirmEmails.Any())
            {
                // Xóa các mã xác nhận hết hạn
                _context.confirmEmails.RemoveRange(expiredConfirmEmails);
                _context.SaveChanges();
            }

            // Tạo mã xác nhận mới
            string confirmationCode = GenerateConfirmationCode();

            // Lưu mã xác nhận mới vào cơ sở dữ liệu
            var confirmEmail = new ConfirmEmail
            {
                UserId = user.Id,
                ExpiredTime = DateTime.Now.AddMinutes(10), // Thời gian hết hạn: 10 phút sau khi tạo
                ConfirmCode = confirmationCode,
                IsConfirm = false // Chưa được xác nhận
            };

            _context.confirmEmails.Add(confirmEmail);
            _context.SaveChanges();

            // Gửi email chứa mã xác nhận mới đến địa chỉ email của người đăng ký
            if (SendConfirmationEmail(email, confirmationCode))
            {
                return new ResponseObject<bool>(StatusCodes.Status200OK, "Mã xác nhận đã được gửi thành công!", true);
            }
            else
            {
                // Gửi email thất bại
                return new ResponseObject<bool>(StatusCodes.Status404NotFound, "Đã có lỗi xảy ra khi gửi email xác nhận. Vui lòng thử lại sau!", false);
            }
        }

        public ResponseObject<bool> ResetPassword(string resetCode, string newPassword)
        {
            var confirmEmail = _context.confirmEmails.FirstOrDefault(c => c.ConfirmCode == resetCode);

            if (confirmEmail != null)
            {
                // Kiểm tra xem mã xác nhận còn hạn sử dụng hay không
                if (confirmEmail.ExpiredTime >= DateTime.Now)
                {
                    // Lấy thông tin người dùng từ bảng Users
                    var user = _context.users.FirstOrDefault(u => u.Id == confirmEmail.UserId);
                    if (user != null)
                    {
                        // Cập nhật mật khẩu mới cho người dùng
                        user.Password = BcryptNet.HashPassword(newPassword);
                        _context.SaveChanges();

                        // Đánh dấu mã xác nhận đã được sử dụng
                        confirmEmail.IsConfirm = true;
                        _context.SaveChanges();

                        return new ResponseObject<bool>(StatusCodes.Status200OK, "Mật khẩu đã được cập nhật thành công!", true);
                    }else
                    {
                        return new ResponseObject<bool>(StatusCodes.Status404NotFound, "Không tìm thấy người dùng tương ứng với mã xác nhận.", false);
                    }
                }
                else if (!confirmEmail.IsConfirm)
                {
                    return new ResponseObject<bool>(StatusCodes.Status404NotFound, "Tài khoản email chưa được xác minh!", false);
                }
                else
                {
                    // Mã xác nhận đã hết hạn
                    return new ResponseObject<bool>(StatusCodes.Status404NotFound, "Mã xác minh đã hết hạn. Vui lòng gửi lại mã xác minh!", false);
                }
            }

            return new ResponseObject<bool>(StatusCodes.Status404NotFound, "Xác minh tài khoản thất bại. Người dùng hoặc mã xác minh không hợp lệ!", false);
        }

        public void CleanExpiredResetCodes()
        {
            var expiredCodes = _context.confirmEmails.Where(c => c.ExpiredTime < DateTime.Now).ToList();

            _context.confirmEmails.RemoveRange(expiredCodes);
            _context.SaveChanges();
        }
    }
}
