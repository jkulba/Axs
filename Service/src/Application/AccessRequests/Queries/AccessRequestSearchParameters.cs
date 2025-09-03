namespace Application.AccessRequests.Queries;

public class AccessRequestSearchParameters
{
    public string? SearchTerm { get; set; }
    public string? SearchType { get; set; }
    public string? UserName { get; set; }
    public int? JobNumber { get; set; }
    public int? CycleNumber { get; set; }
    public string? ActivityCode { get; set; }
    public string? ApplicationName { get; set; }
    public string? Workstation { get; set; }
    public DateTime? CreatedFromDate { get; set; }
    public DateTime? CreatedToDate { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}
