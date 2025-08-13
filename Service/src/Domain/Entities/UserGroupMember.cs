using System;

namespace Domain.Entities;

public class UserGroupMember
{
    public int GroupId { get; set; }

    public string UserId { get; set; } = null!;

    // Navigation property
    public virtual UserGroup Group { get; set; } = null!;
}
