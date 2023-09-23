using Microsoft.AspNetCore.Http;

namespace Application.Repository.DTO.Common
{
    public class AdminUserDTO
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public Guid DepartmentId { get; set; }

        public string DepartmentName { get; set; }  

        public string Username { get; set; }

        public string Email { get; set; }

        public byte[] ProfilePicture { get; set; }

        public bool Status { get; set; }
    }
}
