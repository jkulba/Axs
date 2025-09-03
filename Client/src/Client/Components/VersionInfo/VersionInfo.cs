namespace Client.Components.VersionInfo;

public record VersionInfo(string? BuildNumber, string? GitTag, string? GitBranch, string? GitHead, string? BuildHost, string? CommitHash, DateTime BuildDate)
{
    /// <summary>
    /// Formats the BuildDate as a string using the specified format.
    /// </summary>
    /// <param name="format">Optional format string (defaults to "yyyy-MM-dd HH:mm:ss")</param>
    /// <returns>Formatted date string</returns>
    public string GetFormattedBuildDate(string format = "yyyy-MM-dd HH:mm:ss")
    {
        return BuildDate.ToString(format);
    }

    /// <summary>
    /// Returns the build date formatted as a relative time (e.g. "2 days ago")
    /// </summary>
    /// <returns>A human-readable relative time string</returns>
    public string GetRelativeBuildDate()
    {
        var timeSpan = DateTime.UtcNow - BuildDate;

        if (timeSpan.TotalDays > 30)
            return $"{(int)timeSpan.TotalDays / 30} months ago";
        if (timeSpan.TotalDays > 1)
            return $"{(int)timeSpan.TotalDays} days ago";
        if (timeSpan.TotalHours > 1)
            return $"{(int)timeSpan.TotalHours} hours ago";
        if (timeSpan.TotalMinutes > 1)
            return $"{(int)timeSpan.TotalMinutes} minutes ago";

        return "just now";
    }
}