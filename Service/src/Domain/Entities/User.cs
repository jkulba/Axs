using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string GraphId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string PrincipalName { get; set; } = null!;

    public bool IsEnabled { get; set; } = true;

    public DateTime UtcCreatedAt { get; set; }

    public string? CreatedByNum { get; set; }

    public DateTime? UtcUpdatedAt { get; set; }

    public string? UpdatedByNum { get; set; }

    // Navigation properties
    public virtual ICollection<Authorization> Authorizations { get; set; } = new List<Authorization>();

    public virtual ICollection<UserGroupMember> UserGroupMemberships { get; set; } = new List<UserGroupMember>();
}