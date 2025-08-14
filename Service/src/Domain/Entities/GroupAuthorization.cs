using System;

namespace Domain.Entities;

public class GroupAuthorization
{
    public int AuthorizationId { get; set; }

    public int JobNumber { get; set; }

    public int GroupId { get; set; }

    public int ActivityId { get; set; }

    public bool IsAuthorized { get; set; }

    public DateTime UtcCreatedAt { get; set; }

    public string? CreatedByNum { get; set; }

    public DateTime? UtcUpdatedAt { get; set; }

    public string? UpdatedByNum { get; set; }

    // Navigation properties
    public virtual UserGroup Group { get; set; } = null!;

    public virtual Activity Activity { get; set; } = null!;
}
