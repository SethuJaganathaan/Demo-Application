namespace Application.Repository.DTO.User
{
    public class UserRequest
    {
        public Guid UserId { get; set; }

        public Guid? RoleId { get; set; }

        public string RoleName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsLoggedIn { get; set; }

        public bool Status { get; set; }    

    }
}
