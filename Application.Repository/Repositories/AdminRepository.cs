using Application.Repository.DTO.Admin;
using Application.Repository.DTO.User;
using Application.Repository.Entities;
using Application.Repository.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationContext _dbcontext;
        private readonly IMapper _mapper;
        string result = "Action Successful";
        public AdminRepository(ApplicationContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<string> DeleteUser(Guid userId)
        {
            var user = await _dbcontext.Users.FindAsync(userId);
            if (user != null)
            {
                _dbcontext.Users.Remove(user);
                await _dbcontext.SaveChangesAsync();
                return result;
            }
            return "User not found";
        }

        public async Task<UserDTO> GetUserById(Guid userId)
        {
            var query = await (from user in _dbcontext.Users
                        where user.UserId == userId
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
                           Status = user.Status
                        }).FirstOrDefaultAsync();

            return query;
        }

        public async Task<string> UpdateUser(Guid userId, UserUpdateDTO user)
        {
            var existingUser = await _dbcontext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (existingUser == null)
            {
                return "Not Found";
            }

            _mapper.Map(user, existingUser);
            await _dbcontext.SaveChangesAsync();
            return result;
        }
    }
}
