using Api.Extensions;
using Application.Common;
using Application.Users.Commands;
using Domain.Common;
using Domain.Entities;
using FluentValidation;

namespace Api.Endpoints;

internal static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (CreateUserCommand command, ICommandDispatcher commandDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            try
            {
                var result = await commandDispatcher.Dispatch<CreateUserCommand, Result<User>>(command, default);
                return result.IsSuccess ? Results.Created($"/api/users/{result.Value.Id}", result.Value) : result.ToProblemDetails();

            }
            catch (ValidationException validationEx)
            {
                logger.LogWarning("Validation failed for CreateUserCommand: {ValidationErrors}",
                    string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage)));

                return Results.ValidationProblem(
                    validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()),
                    title: "One or more validation errors occurred");
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating user");
                return Results.Problem("Internal server error", statusCode: 500);
            }

        })
        .WithName("CreateUser")
        .WithSummary("Create a new user")
        .WithDescription("Creates a new user.")
        .WithTags("Users")
        .WithOpenApi();

        app.MapPut("/users/{id}", async (int id, UpdateUserCommand command, ICommandDispatcher commandDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            try
            {
                // Ensure the route parameter matches the command Id
                if (id != command.Id)
                {
                    return Results.BadRequest("The user ID in the URL does not match the user ID in the request body.");
                }

                var result = await commandDispatcher.Dispatch<UpdateUserCommand, Result<User>>(command, default);
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            }
            catch (ValidationException validationEx)
            {
                logger.LogWarning("Validation failed for UpdateUserCommand: {ValidationErrors}",
                    string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage)));

                return Results.ValidationProblem(
                    validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()),
                    title: "One or more validation errors occurred");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating user with ID {UserId}", id);
                return Results.Problem("Internal server error", statusCode: 500);
            }

        })
        .WithName("UpdateUser")
        .WithSummary("Update an existing user")
        .WithDescription("Updates an existing user.")
        .WithTags("Users")
        .WithOpenApi();


        app.MapDelete("/users/{id}", async (int id, ICommandDispatcher commandDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            var command = new DeleteUserCommand(id);
            var result = await commandDispatcher.Dispatch<DeleteUserCommand, Result<int>>(command, default);
            return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
        })
        .WithName("DeleteUser")
        .WithSummary("Delete an existing user")
        .WithDescription("Deletes an existing user.")
        .WithTags("Users")
        .WithOpenApi();

        return app;
    }
}