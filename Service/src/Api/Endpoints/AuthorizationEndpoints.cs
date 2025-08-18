using Api.Contracts;
using Api.Extensions;
using Application.Authorization.Commands;
using Application.Common;
using Application.Interfaces;
using Domain.Common;
using FluentValidation;

namespace Api.Endpoints;

internal static class AuthorizationEndpoints
{
    public static IEndpointRouteBuilder MapAuthorizationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/authorization/verify-access", async (
            VerifyAccessRequest request,
            ICommandDispatcher commandDispatcher,
            ILogger<IEndpointRouteBuilder> logger) =>
        {
            try
            {
                var command = new VerifyAccessCommand(
                    request.ProfileUserName,
                    request.JobNumber,
                    request.CycleNumber,
                    request.Workstation,
                    request.ApplicationName,
                    request.ActivityCode ?? string.Empty
                );

                var result = await commandDispatcher.Dispatch<VerifyAccessCommand, Result<AuthorizationResult>>(command, default);
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            }
            catch (ValidationException validationEx)
            {
                logger.LogWarning("Validation failed for VerifyAccessCommand: {ValidationErrors}",
                    string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage)));

                return Results.ValidationProblem(
                    validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()),
                    title: "One or more validation errors occurred");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error verifying access for user {ProfileUserName} and job {JobNumber}",
                    request.ProfileUserName, request.JobNumber);
                return Results.Problem("Internal server error", statusCode: 500);
            }
        })
        .WithName("VerifyAccess")
        .Produces<VerifyAccessResponse>(StatusCodes.Status200OK)
        .WithSummary("Verify access for a user")
        .WithDescription("Verifies if a user has access to a specific job.")
        .WithTags("Authorization")
        .WithOpenApi();

        return app;
    }
}
