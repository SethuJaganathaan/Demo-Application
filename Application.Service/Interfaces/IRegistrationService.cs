using Application.Repository.DTO.Common;

namespace Application.Service.Interfaces
{
    public interface IRegistrationService
    {
        Task<string> SignUp(AdminUserCreateDTO user);
    }
}
