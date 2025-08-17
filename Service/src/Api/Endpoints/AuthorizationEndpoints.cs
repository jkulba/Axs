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
            IValidator<VerifyAccessRequest> validator,
            ICommandDispatcher commandDispatcher,
            ILogger<IEndpointRouteBuilder> logger) =>
        {
            // Validate the request
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(error => new
                    {
                        Field = error.PropertyName,
                        Message = error.ErrorMessage
                    });

                return Results.ValidationProblem(validationResult.ToDictionary());
            }

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
