using System;
using System.Collections.Generic;

namespace Application.Repository.Entities;

public partial class Department
{
    public Guid DepartmentId { get; set; }

    public string DepartmentName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
