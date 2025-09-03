using Refit;
using System.Text.Json;

namespace Client.Services;

public interface IAccessRequestApi
{
    [Get("/api/access-requests/search")]
    Task<IEnumerable<AccessRequest>> SearchAccessRequestsQuery([Query] AccessRequestSearchParameters parameters, CancellationToken cancellationToken = default);

    [Get("/api/access-requests/{id}")]
    Task<AccessRequest> GetAccessRequestByIdQuery(int id, CancellationToken cancellationToken = default);

    [Get("/api/access-requests/request-code/{requestCode}")]
    Task<AccessRequest> GetAccessRequestByRequestCodeQuery(Guid requestCode, CancellationToken cancellationToken = default);

    [Get("/api/access-requests/job-number/{jobNumber}")]
    Task<IEnumerable<AccessRequest>> GetAccessRequestsByJobNumberQuery(int jobNumber, CancellationToken cancellationToken = default);

    [Get("/api/access-requests/user-name/{userName}")]
    Task<IEnumerable<AccessRequest>> GetAccessRequestsByUserNameQuery(string userName, CancellationToken cancellationToken = default);

    [Get("/api/access-requests/")]
    Task<IEnumerable<AccessRequest>> GetAccessRequestsQuery(CancellationToken cancellationToken = default);

}

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

public record AccessRequest(
    int Id,
    Guid RequestCode,
    string UserName,
    int JobNumber,
    int CycleNumber,
    string? ActivityCode,
    string? ApplicationName,
    string? Workstation,
    DateTime? UtcCreatedAt
);
