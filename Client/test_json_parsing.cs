using System.Text.Json;
using Client.Services;

// Test the JSON parsing with your example
var json = """{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.1","title":"One or more validation errors occurred","status":400,"errors":{"GraphId":["Graph ID is required.","Graph ID must be a valid GUID format."]}}""";

var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

try
{
    var errorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(json, options);

    Console.WriteLine("Parsed successfully!");
    Console.WriteLine($"Type: {errorResponse?.Type}");
    Console.WriteLine($"Title: {errorResponse?.Title}");
    Console.WriteLine($"Status: {errorResponse?.Status}");

    if (errorResponse?.Errors != null)
    {
        Console.WriteLine("Validation Errors:");
        foreach (var kvp in errorResponse.Errors)
        {
            Console.WriteLine($"  {kvp.Key}:");
            foreach (var error in kvp.Value)
            {
                Console.WriteLine($"    - {error}");
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error parsing JSON: {ex.Message}");
}
