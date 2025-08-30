using System;

namespace Domain.Entities;

public class UserGroupMember
{
    public int GroupId { get; set; }

    public string UserId { get; set; } = null!;

    // Navigation properties
    public virtual UserGroup Group { get; set; } = null!;
    public virtual User? User { get; set; }
}
