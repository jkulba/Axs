using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Errors;

public static class ActivityErrors
{
    public static Error ActivityNotFound => new(
        "Activity.ActivityNotFound", "Activity not found");
}