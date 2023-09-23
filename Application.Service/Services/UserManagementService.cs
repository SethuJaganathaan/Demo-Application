using Application.Repository.Interfaces;
using Application.Service.Interfaces;

namespace Application.Service.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserManagementRepository _userManagementRepository;
        public UserManagementService(IUserManagementRepository userManagementRepository)
        {
            _userManagementRepository = userManagementRepository;
        }

        public async Task<object> UserManagementByRoleid(Guid roleid)
        {
            return await _userManagementRepository.UserManagementByRoleid(roleid);
        }
    }
}
