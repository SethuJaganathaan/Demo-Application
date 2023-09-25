using Application.Repository.DTO.Common;
using Application.Repository.DTO.User;

namespace Application.Service.Interfaces
{
    public interface IRegistrationService
    {
        Task<string> SignUp(AdminUserCreateDTO user);

        Task<BaseResponse> Login(LoginDTO login);

        Task<UserRequest> GetUser(string email);
    }
}
