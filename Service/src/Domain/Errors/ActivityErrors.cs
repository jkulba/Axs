using Domain.Common;

namespace Domain.Errors;

public static class ActivityErrors
{
    public static Error ActivitiesNotFound => new(
        "Activity.ActivitiesNotFound", "No activities found");

    public static Error ActivityByIdNotFound(int id) => new(
        "Activity.ActivityByIdNotFound", $"Activity with ID = '{id}' was not found");
}
