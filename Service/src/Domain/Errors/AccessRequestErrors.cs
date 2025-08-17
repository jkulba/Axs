using Domain.Common;

namespace Domain.Errors;

public static class AccessRequestErrors
{
    public static Error AccessRequestsNotFound => new(
        "AccessRequests.AccessRequestsNotFound", "No access requests found");
}