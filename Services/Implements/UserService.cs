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
                || (request.Point == 0)
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
            var user = new User();
            user.Point = request.Point;
            user.Email = request.Email;
            user.UserName = request.UserName;
            user.Password = BcryptNet.HashPassword(request.Password);
            user.Name = request.Name;
            user.PhoneNumber = request.PhoneNumber;
            user.RankCustomerId = request.RankCustomerId;
            user.UserStatusId = request.UserStatusId;
            user.RoleId = 3;
            user.IsActive = true;
            _context.users.Add(user);
            _context.SaveChanges();
            DataResponseUser result = _converter.EntityToDTO(user);
            return _responseObject.ResponseSuccess("Đăng ký tài khoản thành công", result);
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
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("RoleId", user.RoleId.ToString()),
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
            if(string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }

            bool checkPass = BcryptNet.Verify(request.Password, user.Password);
            if(!checkPass)
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Mật khẩu không chính xác", null);
            }
            return _responseTokenObject.ResponseSuccess("Đăng nhập thành công", GenerateAccessToken(user));
        }

        public IQueryable<DataResponseUser> GetAll()
        {
            var result = _context.users.Select(x => _converter.EntityToDTO(x));
            return result;
        }
    }
}
