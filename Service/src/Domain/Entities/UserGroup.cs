using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class UserGroup
{
    public int GroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public string? Description { get; set; }

    public string? GroupOwner { get; set; }

    // Navigation properties
    public virtual ICollection<UserGroupMember> Members { get; set; } = new List<UserGroupMember>();

    public virtual ICollection<GroupAuthorization> GroupAuthorizations { get; set; } = new List<GroupAuthorization>();
}
