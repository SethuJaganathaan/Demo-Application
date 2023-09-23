namespace Application.Repository.Entities;

public partial class Admin
{
    public int Id { get; set; }

    public Guid AdminId { get; set; }

    public string AdminName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public Guid Roleid { get; set; }

    public Guid UserId { get; set; }

    public virtual Role Role { get; set; }

    public virtual User User { get; set; }
}
