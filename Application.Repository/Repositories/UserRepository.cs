using Application.Repository.DTO.Common;
using Application.Repository.DTO.User;
using Application.Repository.Entities;
using Application.Repository.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _dbcontext;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            var query = await (from user in _dbcontext.Users
                        join role in _dbcontext.Roles on user.RoleId equals role.RoleId
                        join department in _dbcontext.Departments on user.DepartmentId equals department.DepartmentId
                        select new UserDTO
                        {
                           UserId = user.UserId,
                           RoleName = role.Rolename,
                           DepartmentName = department.DepartmentName,
                           Username = user.Username,
                           Email = user.Email,
                           ProfilePicture = user.ProfilePicture,
                           Status = user.Status,
                        }).ToListAsync();

            var userDTO = _mapper.Map<List<UserDTO>>(query);
            return userDTO;
        }

        public async Task<bool> SoftDeleteUser(Guid userId)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user != null)
            {
                await _dbcontext.SaveChangesAsync();
                return true; 
            }
            return false;
        }

    }
}
