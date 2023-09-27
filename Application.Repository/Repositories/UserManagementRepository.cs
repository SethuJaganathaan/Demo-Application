using Application.Repository.DTO.Common;
using Application.Repository.Entities;
using Application.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository.Repositories
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly ApplicationContext _dbcontext;
        public UserManagementRepository(ApplicationContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<object> UserManagementByRoleid(Guid Roleid)
        {
            var superAdminRole = await _dbcontext.Roles
                .FirstOrDefaultAsync(r => r.Rolename == "SuperAdmin");

            if (superAdminRole == null)
            {
                return new { Message = "SuperAdmin role not found" };
            }

            bool isSuperAdmin = Roleid == superAdminRole.RoleId;

            return isSuperAdmin
                ? await (from Roles in _dbcontext.Roles
                         join users in _dbcontext.Users on Roles.RoleId equals users.RoleId
                         select new AdminUserDTO
                         {
                             UserId = users.UserId,
                             DepartmentId = users.DepartmentId ?? Guid.Empty,
                             Username = users.Username,
                             Email = users.Email,
                             ProfilePicture = users.ProfilePicture,
                         }).ToListAsync()
                : new
                {
                    UserCount = await _dbcontext.Users.CountAsync(u => u.RoleId == Roleid),
                    SuperAdminCount = await _dbcontext.Users.CountAsync(u => u.RoleId == superAdminRole.RoleId)
                };
        }
    }
}
