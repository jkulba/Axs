using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Authorization
{
    public int AuthorizationId { get; set; }

    public int JobNumber { get; set; }

    public string UserId { get; set; } = null!;

    public int ActivityId { get; set; }

    public bool IsAuthorized { get; set; }

    public DateTime UtcCreatedAt { get; set; }

    public string? CreatedByNum { get; set; }

    public DateTime? UtcUpdatedAt { get; set; }

    public string? UpdatedByNum { get; set; }

    // Navigation properties
    public virtual Activity Activity { get; set; } = null!;
}
