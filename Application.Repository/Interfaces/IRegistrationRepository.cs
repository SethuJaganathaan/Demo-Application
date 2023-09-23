using Application.Repository.DTO.Common;

namespace Application.Repository.Interfaces
{
    public interface IRegistrationRepository
    {
        Task<string> SignUp(AdminUserCreateDTO user);

        Task<BaseResponse> Login(LoginDTO login);

    }
}
