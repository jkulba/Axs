using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class AccessRequest
{
    public int RequestId { get; set; }

    public Guid RequestCode { get; set; }

    public string UserName { get; set; } = null!;

    public int JobNumber { get; set; }

    public int CycleNumber { get; set; }

    public string? ActivityCode { get; set; }

    public string? Application { get; set; }

    public string? Version { get; set; }

    public string? Machine { get; set; }

    public DateTime? UtcCreatedAt { get; set; }

}
