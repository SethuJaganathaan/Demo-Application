using Application.Repository.DTO.User;

namespace Application.Service.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsers();

        Task<bool?> SoftDeleteUser(Guid userId);

    }
}
