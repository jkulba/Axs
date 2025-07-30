using System;
using System.Collections.Generic;

namespace Domain.Entities;

/// <summary>
/// Stores access groups for profile authorization
/// </summary>
public class AccessGroup
{
    public int GroupId { get; set; }

    public Guid GroupCode { get; set; }

    public string? GroupName { get; set; }

    public DateTime? UtcExpirationDate { get; set; }

    public DateTime? UtcCreatedAt { get; set; }

    public string? CreatedByNum { get; set; }
}
