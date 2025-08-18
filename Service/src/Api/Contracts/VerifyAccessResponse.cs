namespace Api.Contracts;

public record VerifyAccessResponse(int Id, Guid AccessRequestCode, int JobNumber, int CycleNumber, string Requestor, string ApprovalStatus);

// Id = accessRequest.AccessRequestId,
// AccessRequestCode = accessRequest.AccessRequestCode,
// JobNumber = accessRequest.JobNumber,
// CycleNumber = accessRequest.CycleNumber,
// Requestor = accessRequest.Requestor,
// ApprovalStatus = accessRequest.ApprovalStatus?.Name
