using Application.Repository.DTO.User;
using Application.Repository.Enums;

namespace Application.Service.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsers();

        Task<bool> SoftDeleteUser(Guid userId);

    }
}
