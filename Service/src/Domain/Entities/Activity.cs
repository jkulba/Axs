using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Activity
{
    public int ActivityId { get; set; }

    public string ActivityCode { get; set; } = null!;

    public string ActivityName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Authorization> Authorizations { get; set; } = new List<Authorization>();

    public virtual ICollection<GroupAuthorization> GroupAuthorizations { get; set; } = new List<GroupAuthorization>();
}
