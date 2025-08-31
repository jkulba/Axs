using Domain.Common;

namespace Domain.Errors;

public static class AccessRequestErrors
{
    public static Error AccessRequestsNotFound => new(
        "AccessRequests.AccessRequestsNotFound", "No access requests found");

    public static Error AccessRequestByRequestCodeNotFound(Guid requestCode) => new(
        "AccessRequests.AccessRequestByRequestCodeNotFound", $"Access request with Request Code = '{requestCode}' was not found");

    public static Error AccessRequestByIdNotFound(int id) => new(
        "AccessRequests.AccessRequestByIdNotFound", $"Access request with ID = '{id}' was not found");

    public static Error AccessRequestByUserNameNotFound(string userName) => new(
        "AccessRequests.AccessRequestByUserNameNotFound", $"Access request for User Name = '{userName}' was not found");

    public static Error AccessRequestByUserIdNotFound(int userId) => new(
        "AccessRequests.AccessRequestByUserIdNotFound", $"Access request for User ID = '{userId}' was not found");

    public static Error AccessRequestByJobNumberNotFound(int jobNumber) => new(
        "AccessRequests.AccessRequestByJobNumberNotFound", $"Access request for Job Number = '{jobNumber}' was not found");

}