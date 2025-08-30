using Api.Extensions;
using Application.Activities.Commands;
using Application.Activities.Queries;
using Application.Common;
using Domain.Common;
using Domain.Entities;
using FluentValidation;

namespace Api.Endpoints;

internal static class ActivityEndpoints
{
    public static IEndpointRouteBuilder MapActivityEndpoints(this IEndpointRouteBuilder app)
    {

        // Create Activity
        app.MapPost("/api/activities", async (CreateActivityCommand command, ICommandDispatcher commandDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            try
            {
                var result = await commandDispatcher.Dispatch<CreateActivityCommand, Result<Activity>>(command, default);
                return result.IsSuccess ? Results.Created($"/api/activities/{result.Value.Id}", result.Value) : result.ToProblemDetails();

            }
            catch (ValidationException validationEx)
            {
                logger.LogWarning("Validation failed for CreateActivityCommand: {ValidationErrors}",
                    string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage)));

                return Results.ValidationProblem(
                    validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()),
                    title: "One or more validation errors occurred");
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating activity");
                return Results.Problem("Internal server error", statusCode: 500);
            }
        })
        .WithName("CreateActivity")
        .WithSummary("Create a new activity")
        .WithDescription("Creates a new activity.")
        .WithTags("Activities")
        .WithOpenApi();

        // Update Activity
        app.MapPut("/api/activities/{id:int}", async (int id, UpdateActivityCommand command, ICommandDispatcher commandDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            try
            {
                // Ensure the route parameter matches the command Id
                if (id != command.Id)
                {
                    return Results.BadRequest("The activity ID in the URL does not match the activity ID in the request body.");
                }

                var result = await commandDispatcher.Dispatch<UpdateActivityCommand, Result<Activity>>(command, default);
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            }
            catch (ValidationException validationEx)
            {
                logger.LogWarning("Validation failed for UpdateActivityCommand: {ValidationErrors}",
                    string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage)));

                return Results.ValidationProblem(
                    validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()),
                    title: "One or more validation errors occurred");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating activity with ID {ActivityId}", id);
                return Results.Problem("Internal server error", statusCode: 500);
            }
        })
        .WithName("UpdateActivity")
        .WithSummary("Update an existing activity")
        .WithDescription("Updates an existing activity by ID.")
        .WithTags("Activities")
        .WithOpenApi();

        // Get Activity by Id
        app.MapGet("/api/activities/{id:int}", async (int id, IQueryDispatcher queryDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            var query = new GetActivityByIdQuery(id);
            var result = await queryDispatcher.Dispatch<GetActivityByIdQuery, Result<Activity>>(query, default);

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .WithName("GetActivityById")
        .WithSummary("Get an activity by ID")
        .WithDescription("Retrieves a specific activity by ID.")
        .WithTags("Activities")
        .WithOpenApi();

        // Get All Activities
        app.MapGet("/api/activities", async (IQueryDispatcher queryDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            var query = new GetActivitiesQuery();
            var result = await queryDispatcher.Dispatch<GetActivitiesQuery, Result<IEnumerable<Activity>>>(query, default);

            if (result.IsNotFound)
            {
                return Results.NotFound($"Activities not found.");
            }

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();

        })
        .WithName("GetActivities")
        .WithSummary("Get all activities")
        .WithDescription("Retrieves a list of all activities.")
        .WithTags("Activities")
        .WithOpenApi();


        return app;
    }
}