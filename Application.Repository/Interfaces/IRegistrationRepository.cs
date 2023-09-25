using Application.Repository.DTO.Common;
using Application.Repository.DTO.User;

namespace Application.Repository.Interfaces
{
    public interface IRegistrationRepository
    {
        Task<string> SignUp(AdminUserCreateDTO user);

        Task<BaseResponse> Login(LoginDTO login);

        Task<UserRequest> GetUser(string email);

        Task<bool> UpdateUserLoggedInStatus(Guid userId,bool Status);

    }
}
