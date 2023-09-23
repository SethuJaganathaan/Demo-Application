using Application.Repository.DTO.User;

namespace Application.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserDTO>> GetAllUsers();
    }
}
