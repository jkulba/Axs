using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class AccessRequest
{
    public int RequestId { get; set; }

    public Guid RequestCode { get; set; }

    public string EmployeeNum { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public int JobNumber { get; set; }

    public int CycleNumber { get; set; }

    public string? JobSiteCode { get; set; }

    public string? JobManufacturingSiteCode { get; set; }

    public string? ApproverNum { get; set; }

    public int ApprovalStatus { get; set; }

    public DateTime? UtcCreatedAt { get; set; }

    public string? CreatedByNum { get; set; }

    public DateTime? UtcUpdatedAt { get; set; }

    public string? UpdatedByNum { get; set; }

    public DateTime? AccessExpiresAt { get; set; }
}
