using Application.Repository.Context;
using Application.Repository.DTO.Common;
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
            var roleid = new SqlParameter("roleid", login.roleid);
            var email = new SqlParameter("@email", login.email);
            var password = new SqlParameter("@Password", login.password);
            var sql = "EXEC UserLogin @roleid, @email, @Password";

            List<SqlParameter> parameters = new List<SqlParameter>
        {
            roleid,
            email,
            password
        };

            var result = await _dbcontext.Database.SqlQueryRaw<string>(sql, parameters.ToArray()).ToListAsync();
            string status = result.FirstOrDefault() ?? string.Empty;

            if (status == "LOGIN SUCCESSFUL")
            {
                var tokenhandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_setting.SecurityKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Email, login.email)
                    }),
                    Expires = DateTime.Now.AddHours(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenhandler.CreateToken(tokenDescriptor);
                string finalToken = tokenhandler.WriteToken(token);

                return new BaseResponse 
                {
                    Status = "Success",
                    Token = finalToken
                };
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
    }
}
