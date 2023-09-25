using Application.Repository.Context;
using Application.Repository.DTO.Common;
using Application.Repository.DTO.User;
using Application.Repository.Entities;
using Application.Repository.Interfaces;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Repository.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly ApplicationContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly JWTSetting _setting;

        string result = "Action Successful";
        public RegistrationRepository(ApplicationContext dbcontext, IMapper mapper, IOptions<JWTSetting> options)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _setting = options.Value;
        }

        public async Task<BaseResponse> Login(LoginDTO login)
        {
            var email = new SqlParameter("@email", login.email);
            var password = new SqlParameter("@Password", login.password);
            var sql = "EXEC UserLogin @email, @Password";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                email,
                password
            };

            var result = await _dbcontext.Database.SqlQueryRaw<string>(sql, parameters.ToArray()).ToListAsync();
            string status = result.FirstOrDefault() ?? string.Empty;

            if (status == "LOGIN SUCCESSFUL")
            {
                var user = await GetUser(login.email);
                if (user != null)
                {
                    await UpdateUserLoggedInStatus(user.UserId, true);
                    await _dbcontext.SaveChangesAsync();

                    var token = GenerateToken(user);

                    return new BaseResponse
                    {
                        Status = "LOGIN SUCCESSFUL",
                        Token = token
                    };
                }
            }
            return new BaseResponse
            {
                Status = "Failure",
                Token = null
            };
        }

        public async Task<string> SignUp(AdminUserCreateDTO user)
        {
            if (await _dbcontext.Users.AnyAsync(u => u.Email == user.Email))
                return "Email already exist";

            if (user.ProfilePicture != null)
            {
                string[] allowedExtensions = { ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(user.ProfilePicture.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                    return "Invalid picture format";
            }

            var newUser = _mapper.Map<User>(user);
            using (var memoryStream = new MemoryStream())
            {
                await user.ProfilePicture.CopyToAsync(memoryStream);
                newUser.ProfilePicture = memoryStream.ToArray();
            }

            _dbcontext.Users.Add(newUser);
            await _dbcontext.SaveChangesAsync();
            return result;
        }

        public async Task<UserRequest> GetUser(string email)
        {
            var user = await _dbcontext.Users
                .Include(u => u.Role) 
                .FirstOrDefaultAsync(u => u.Email == email);

            var userResponse = new UserRequest
            {
                UserId = user.UserId,
                RoleId = user.RoleId,
                Email = user.Email,
                Username = user.Username,
                RoleName = user.Role != null ? user.Role.Rolename : null,
            };

            return userResponse;
        }

        public string GenerateToken(UserRequest user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_setting.SecurityKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("RoleId", user.RoleId.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("Username", user.Username),
                    new Claim("RoleName", user.RoleName)
                }),
                Expires = DateTime.Now.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> UpdateUserLoggedInStatus(Guid userId,bool Status)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user != null)
            {
                user.Status = Status;
                await _dbcontext.SaveChangesAsync();
                return true; 
            }
            return false; 
        }

    }
}
