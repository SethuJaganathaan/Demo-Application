using Application.Repository.DTO.Common;
using Application.Repository.DTO.User;
using Application.Repository.Entities;
using Application.Repository.Enums;
using Application.Repository.Interfaces;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Repository.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly ApplicationContext _dbcontext;
        private readonly IMapper _mapper;
        string result = "Action Successful";
        public RegistrationRepository(ApplicationContext dbcontext, IMapper mapper, IOptions<JWTSetting> options)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
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
                    await UpdateUserStatus(user.UserId, UserStatus.Active);
                    await _dbcontext.SaveChangesAsync();

                    return new BaseResponse
                    {
                        Status = "LOGIN SUCCESSFUL",
                    };
                }
            }
            return new BaseResponse
            {
                Status = "Failure",
            };
        }

        public async Task<string> SignUp(AdminUserCreateDTO user)
        {
            if (await _dbcontext.Users.AnyAsync(u => u.Email == user.Email))
                return "Email already exist";

            if (user.ProfilePicture != null)
            {
                string[] pictureExtensions = { ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(user.ProfilePicture.FileName).ToLower();

                if (!pictureExtensions.Contains(fileExtension))
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

        public async Task<bool> UpdateUserStatus(Guid userId, UserStatus status)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                user.Status = (short)(status == UserStatus.Active ? UserStatus.Active : UserStatus.Inactive);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
