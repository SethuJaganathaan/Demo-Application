using Application.Repository.Context;
using Application.Repository.DTO.User;
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
            var users = await _dbcontext.Users.ToListAsync();
            var userDTO = _mapper.Map<List<UserDTO>>(users);
            return userDTO;
        }
    }
}
