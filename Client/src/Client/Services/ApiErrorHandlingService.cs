using Refit;
using System.Net;
using System.Text.Json;

namespace Client.Services;

public interface IApiErrorHandlingService
{
    ApiErrorInfo ParseApiError(Exception exception);
    string FormatErrorMessage(ApiErrorInfo errorInfo);
}

public class ApiErrorHandlingService : IApiErrorHandlingService
{
    public ApiErrorInfo ParseApiError(Exception exception)
    {
        var errorInfo = new ApiErrorInfo
        {
            Exception = exception,
            ErrorMessage = exception.Message,
            Timestamp = DateTime.UtcNow
        };

        // Handle Refit ApiException specifically
        if (exception is ApiException apiException)
        {
            errorInfo.HttpStatusCode = apiException.StatusCode;
            errorInfo.StatusCodeName = apiException.StatusCode.ToString();

            // Try to parse the error content if available
            if (!string.IsNullOrWhiteSpace(apiException.Content))
            {
                try
                {
                    var errorResponse = ParseErrorContent(apiException.Content);
                    if (errorResponse != null)
                    {
                        errorInfo.ServerErrorMessage = errorResponse.Description ?? errorResponse.Title ?? errorResponse.Detail;
                        errorInfo.ErrorCode = errorResponse.Status?.ToString();
                        errorInfo.ErrorType = errorResponse.Type;
                        errorInfo.TraceId = errorResponse.TraceId;
                        errorInfo.Instance = errorResponse.Instance;
                        errorInfo.ValidationErrors = errorResponse.Errors;
                        errorInfo.RawErrorContent = apiException.Content;
                    }
                }
                catch (JsonException)
                {
                    // If JSON parsing fails, use raw content
                    errorInfo.RawErrorContent = apiException.Content;
                    errorInfo.ServerErrorMessage = apiException.Content;
                }
            }

            // Set user-friendly error message based on status code
            errorInfo.UserFriendlyMessage = GetUserFriendlyMessage(apiException.StatusCode);
        }
        else if (exception is HttpRequestException httpException)
        {
            errorInfo.ErrorMessage = httpException.Message;
            errorInfo.UserFriendlyMessage = "Network error occurred. Please check your connection and try again.";
        }
        else if (exception is TaskCanceledException)
        {
            errorInfo.ErrorMessage = "Request timed out";
            errorInfo.UserFriendlyMessage = "The request timed out. Please try again.";
        }

        return errorInfo;
    }

    public string FormatErrorMessage(ApiErrorInfo errorInfo)
    {
        var parts = new List<string>();

        if (errorInfo.HttpStatusCode.HasValue)
        {
            parts.Add($"HTTP {(int)errorInfo.HttpStatusCode} ({errorInfo.StatusCodeName})");
        }

        // Prioritize validation errors for user-friendly display
        if (errorInfo.ValidationErrors != null && errorInfo.ValidationErrors.Any())
        {
            var validationMessages = new List<string>();
            foreach (var kvp in errorInfo.ValidationErrors)
            {
                foreach (var error in kvp.Value)
                {
                    validationMessages.Add(error);
                }
            }
            parts.Add(string.Join(" ", validationMessages));
        }
        else if (!string.IsNullOrWhiteSpace(errorInfo.ServerErrorMessage))
        {
            parts.Add(errorInfo.ServerErrorMessage);
        }
        else if (!string.IsNullOrWhiteSpace(errorInfo.UserFriendlyMessage))
        {
            parts.Add(errorInfo.UserFriendlyMessage);
        }
        else
        {
            parts.Add(errorInfo.ErrorMessage);
        }

        return string.Join(": ", parts);
    }

    private static ApiErrorResponse? ParseErrorContent(string content)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        try
        {
            // Try to parse as ProblemDetails format (RFC 7807)
            return JsonSerializer.Deserialize<ApiErrorResponse>(content, options);
        }
        catch
        {
            // Try alternative parsing approaches
            try
            {
                // Try to parse as simple error object
                var simpleError = JsonSerializer.Deserialize<SimpleErrorResponse>(content, options);
                if (simpleError != null)
                {
                    return new ApiErrorResponse
                    {
                        Title = simpleError.Error ?? simpleError.Message,
                        Description = simpleError.Description ?? simpleError.Message,
                        Detail = simpleError.Details
                    };
                }
            }
            catch
            {
                // If all parsing fails, return null
            }
        }

        return null;
    }

    private static string GetUserFriendlyMessage(HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.BadRequest => "The request was invalid. Please check the provided data.",
            HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
            HttpStatusCode.Forbidden => "Access to this resource is forbidden.",
            HttpStatusCode.NotFound => "The requested resource was not found.",
            HttpStatusCode.Conflict => "The request conflicts with the current state of the resource.",
            HttpStatusCode.UnprocessableEntity => "The request data is invalid or incomplete.",
            HttpStatusCode.InternalServerError => "An internal server error occurred. Please try again later.",
            HttpStatusCode.BadGateway => "The server received an invalid response. Please try again.",
            HttpStatusCode.ServiceUnavailable => "The service is temporarily unavailable. Please try again later.",
            HttpStatusCode.GatewayTimeout => "The server took too long to respond. Please try again.",
            _ => "An error occurred while processing your request."
        };
    }
}

public class ApiErrorInfo
{
    public Exception Exception { get; set; } = null!;
    public string ErrorMessage { get; set; } = string.Empty;
    public string? UserFriendlyMessage { get; set; }
    public string? ServerErrorMessage { get; set; }
    public HttpStatusCode? HttpStatusCode { get; set; }
    public string? StatusCodeName { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorType { get; set; }
    public string? TraceId { get; set; }
    public string? Instance { get; set; }
    public string? RawErrorContent { get; set; }
    public Dictionary<string, List<string>>? ValidationErrors { get; set; }
    public DateTime Timestamp { get; set; }
}

// Standard ProblemDetails format (RFC 7807) with validation errors
public class ApiErrorResponse
{
    public string? Type { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Detail { get; set; }
    public int? Status { get; set; }
    public string? Instance { get; set; }
    public string? TraceId { get; set; }
    public Dictionary<string, List<string>>? Errors { get; set; }
}

// Alternative error response formats
public class SimpleErrorResponse
{
    public string? Error { get; set; }
    public string? Message { get; set; }
    public string? Description { get; set; }
    public string? Details { get; set; }
}
