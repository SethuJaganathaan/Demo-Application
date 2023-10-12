using Application.Repository.DTO.Common;
using Application.Repository.DTO.User;
using Application.Repository.Enums;

namespace Application.Repository.Interfaces
{
    public interface IRegistrationRepository
    {
        Task<string> SignUp(AdminUserCreateDTO user);

        Task<BaseResponse> Login(LoginDTO login);

        Task<UserRequest> GetUser(string email);

        Task<bool> UpdateUserStatus(Guid userId, UserStatus status);
    }
}
