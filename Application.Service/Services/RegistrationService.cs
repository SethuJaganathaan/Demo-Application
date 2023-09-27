using Application.Repository.DTO.Common;
using Application.Repository.DTO.User;
using Application.Repository.Interfaces;
using Application.Service.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Service.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly JWTSetting _jwtSetting;
        public RegistrationService(IRegistrationRepository registrationRepository, IOptions<JWTSetting> jwtSettingOptions)
        {
            _registrationRepository = registrationRepository;
            _jwtSetting = jwtSettingOptions.Value;
        }

        public async Task<BaseResponse> Login(LoginDTO login)
        {
            var response = await _registrationRepository.Login(login);

            if (response.Status == "LOGIN SUCCESSFUL")
            {
                var user = await _registrationRepository.GetUser(login.email);
                if (user != null)
                {
                    var token = GenerateToken(user); 
                    response.Token = token;
                }
            }
            return response;
        }

        public async Task<string> SignUp(AdminUserCreateDTO user)
        {
            return await _registrationRepository.SignUp(user);
        }

        public async Task<UserRequest> GetUser(string email)
        {
            return await _registrationRepository.GetUser(email);
        }

        public string GenerateToken(UserRequest user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSetting.SecurityKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("RoleId", user.RoleId.ToString()),
                new Claim("Email", user.Email),
                new Claim("Username", user.Username),
                new Claim(ClaimTypes.Role, user.RoleName)
                }),
                Expires = DateTime.Now.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
