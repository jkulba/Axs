using Api.Extensions;
using Application.Common;
using Application.Users.Commands;
using Application.Users.Queries;
using Domain.Common;
using Domain.Entities;
using FluentValidation;

namespace Api.Endpoints;

internal static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users", async (CreateUserCommand command, ICommandDispatcher commandDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
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

        app.MapPut("/api/users/{id}", async (int id, UpdateUserCommand command, ICommandDispatcher commandDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
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


        app.MapDelete("/api/users/{id}", async (int id, ICommandDispatcher commandDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
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

        // Get All Users
        app.MapGet("/api/users", async (IQueryDispatcher queryDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            var query = new GetUsersQuery();
            var result = await queryDispatcher.Dispatch<GetUsersQuery, Result<IEnumerable<User>>>(query, default);

            // if (result.IsNotFound)
            // {
            //     return Results.NotFound($"Users not found.");
            // }

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();

        })
        .WithName("GetUsers")
        .WithSummary("Get all users")
        .WithDescription("Retrieves a list of all users.")
        .WithTags("Users")
        .WithOpenApi();

        // Get User by Id
        app.MapGet("/api/users/{id:int}", async (int id, IQueryDispatcher queryDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            var query = new GetUserByIdQuery(id);
            var result = await queryDispatcher.Dispatch<GetUserByIdQuery, Result<User>>(query, default);

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .WithName("GetUserById")
        .WithSummary("Get a user by ID")
        .WithDescription("Retrieves a specific user by ID.")
        .WithTags("Users")
        .WithOpenApi();


        // Get User by UserId
        app.MapGet("/api/users/userid/{userId}", async (string userId, IQueryDispatcher queryDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            var query = new GetUserByUserIdQuery(userId);
            var result = await queryDispatcher.Dispatch<GetUserByUserIdQuery, Result<User>>(query, default);

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .WithName("GetUserByUserId")
        .WithSummary("Get a user by UserId")
        .WithDescription("Retrieves a specific user by UserId.")
        .WithTags("Users")
        .WithOpenApi();


        return app;
    }
}