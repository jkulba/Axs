using Domain.Common;

namespace Domain.Errors;

public static class UserErrors
{
    public static Error UserNotFound => new(
        "User.UserNotFound", "User not found");
}