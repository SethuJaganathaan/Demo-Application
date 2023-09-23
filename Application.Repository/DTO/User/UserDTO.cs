using Application.Repository.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Repository.DTO.User
{
    public class UserDTO
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

        public Guid DepartmentId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public byte[] ProfilePicture { get; set; }

        public bool Status { get; set; }
    }
}
