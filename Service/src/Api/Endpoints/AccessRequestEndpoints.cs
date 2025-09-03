using Api.Contracts;
using Api.Extensions;
using Application.AccessRequests.Commands;
using Application.AccessRequests.Queries;
using Application.Common;
using Domain.Common;
using Domain.Entities;
using FluentValidation;

namespace Api.Endpoints;

internal static class AccessRequestEndpoints
{
    public static IEndpointRouteBuilder MapAccessRequestEndpoints(this IEndpointRouteBuilder app)
    {

        app.MapGet("/api/access-requests", async (IQueryDispatcher queryDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            var query = new GetAccessRequestsQuery();
            var result = await queryDispatcher.Dispatch<GetAccessRequestsQuery, Result<IEnumerable<AccessRequest>>>(query, default);

            if (result.IsNotFound)
            {
                return Results.NotFound($"Access requests not found.");
            }

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .WithName("GetAccessRequests")
        .WithSummary("Get all access requests")
        .WithDescription("Returns a list of all access requests.")
        .WithTags("AccessRequests")
        .WithOpenApi();

        app.MapGet("/api/access-requests/search", async (
            string? searchTerm,
            string? searchType,
            string? userName,
            int? jobNumber,
            int? cycleNumber,
            string? activityCode,
            string? applicationName,
            string? workstation,
            DateTime? createdFromDate,
            DateTime? createdToDate,
            int? pageNumber,
            int? pageSize,
            IQueryDispatcher queryDispatcher,
            ILogger<IEndpointRouteBuilder> logger) =>
        {
            try
            {
                var searchParameters = new Api.Contracts.AccessRequestSearchParameters
                {
                    SearchTerm = searchTerm,
                    SearchType = searchType,
                    UserName = userName,
                    JobNumber = jobNumber,
                    CycleNumber = cycleNumber,
                    ActivityCode = activityCode,
                    ApplicationName = applicationName,
                    Workstation = workstation,
                    CreatedFromDate = createdFromDate,
                    CreatedToDate = createdToDate,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                var query = new SearchAccessRequestsQuery(searchParameters);
                var result = await queryDispatcher.Dispatch<SearchAccessRequestsQuery, Result<IEnumerable<AccessRequest>>>(query, default);

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error searching access requests with parameters: {@SearchParameters}", new { searchTerm, userName, jobNumber });
                return Results.Problem("Internal server error", statusCode: 500);
            }
        })
        .WithName("SearchAccessRequests")
        .WithSummary("Search access requests")
        .WithDescription("Returns a list of access requests filtered by the provided search parameters.")
        .WithTags("AccessRequests")
        .WithOpenApi();

        app.MapGet("/api/access-requests/{id:int}", async (int id, IQueryDispatcher queryDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            try
            {
                var query = new GetAccessRequestByIdQuery(id);
                var result = await queryDispatcher.Dispatch<GetAccessRequestByIdQuery, Result<AccessRequest>>(query, default);

                if (result.IsNotFound)
                {
                    return Results.NotFound($"Access request with ID {id} not found.");
                }

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving access request with ID {Id}", id);
                return Results.Problem("Internal server error", statusCode: 500);
            }
        })
        .WithName("GetAccessRequestsById")
        .WithSummary("Get access request by ID")
        .WithDescription("Returns a single access request by its ID.")
        .WithTags("AccessRequests")
        .WithOpenApi();

        app.MapGet("/api/access-requests/request-code/{requestCode:guid}", async (Guid requestCode, IQueryDispatcher queryDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            try
            {
                var query = new GetAccessRequestByRequestCodeQuery(requestCode);
                var result = await queryDispatcher.Dispatch<GetAccessRequestByRequestCodeQuery, Result<AccessRequest>>(query, default);
                if (result.IsNotFound)
                    return Results.NotFound($"Access request with code {requestCode} not found.");
                return Results.Ok(result.Value);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving access request with code {RequestCode}", requestCode);
                return Results.Problem("Internal server error", statusCode: 500);
            }
        })
        .WithName("GetAccessRequestsByRequestCode")
        .WithSummary("Get access request by request code")
        .WithDescription("Returns a single access request by its request code.")
        .WithTags("AccessRequests")
        .WithOpenApi();

        app.MapGet("/api/access-requests/job-number/{jobNumber:int}", async (int jobNumber, IQueryDispatcher queryDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            try
            {
                var query = new GetAccessRequestsByJobNumberQuery(jobNumber);
                var result = await queryDispatcher.Dispatch<GetAccessRequestsByJobNumberQuery, Result<IEnumerable<AccessRequest>>>(query, default);

                if (result.IsNotFound)
                {
                    return Results.NotFound($"Access requests not found.");
                }

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving access requests for job number {JobNumber}", jobNumber);
                return Results.Problem("Internal server error", statusCode: 500);
            }
        })
        .WithName("GetAccessRequestsByJobNumber")
        .WithSummary("Get access requests by job number")
        .WithDescription("Returns a list of access requests filtered by job number.")
        .WithTags("AccessRequests")
        .WithOpenApi();

        app.MapGet("/api/access-requests/user-name/{userName}", async (string userName, IQueryDispatcher queryDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            try
            {
                var query = new GetAccessRequestsByUserNameQuery(userName);
                var result = await queryDispatcher.Dispatch<GetAccessRequestsByUserNameQuery, Result<IEnumerable<AccessRequest>>>(query, default);

                if (result.IsNotFound)
                {
                    return Results.NotFound($"Access requests not found.");
                }

                return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving access requests for user {UserName}", userName);
                return Results.Problem("Internal server error", statusCode: 500);
            }
        })
        .WithName("GetAccessRequestsByUserName")
        .WithSummary("Get access requests by user name")
        .WithDescription("Returns a list of access requests filtered by user name.")
        .WithTags("AccessRequests")
        .WithOpenApi();

        app.MapPost("/api/access-requests", async (CreateAccessRequestCommand command, ICommandDispatcher commandDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            try
            {
                var result = await commandDispatcher.Dispatch<CreateAccessRequestCommand, Result<AccessRequest>>(command, default);
                return result.IsSuccess ? Results.Created($"/api/access-requests/{result.Value.Id}", result.Value) : result.ToProblemDetails();
            }
            catch (ValidationException validationEx)
            {
                logger.LogWarning("Validation failed for CreateAccessRequestCommand: {ValidationErrors}",
                    string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage)));

                return Results.ValidationProblem(
                    validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()),
                    title: "One or more validation errors occurred");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating access request");
                return Results.Problem("Internal server error", statusCode: 500);
            }
        })
        .WithName("CreateAccessRequest")
        .WithSummary("Create a new access request")
        .WithDescription("Creates a new access request.")
        .WithTags("AccessRequests")
        .WithOpenApi();

        app.MapDelete("/api/access-requests/{id:int}", async (int id, ICommandDispatcher commandDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            var command = new DeleteAccessRequestCommand(id);
            var result = await commandDispatcher.Dispatch<DeleteAccessRequestCommand, Result<int>>(command, default);
            return result.IsSuccess ? Results.NoContent() : result.ToProblemDetails();
        })
        .WithName("DeleteAccessRequest")
        .WithSummary("Delete an existing access request")
        .WithDescription("Deletes an existing access request.")
        .WithTags("AccessRequests")
        .WithOpenApi();

        app.MapGet("/api/access-requests/exists/{requestCode:guid}", async (Guid requestCode, IQueryDispatcher queryDispatcher, ILogger<IEndpointRouteBuilder> logger) =>
        {
            try
            {
                var query = new ExistsAccessRequestByRequestCodeQuery(requestCode);
                var exists = await queryDispatcher.Dispatch<ExistsAccessRequestByRequestCodeQuery, Result<bool>>(query, default);
                return Results.Ok(exists);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking if access request exists with code {RequestCode}", requestCode);
                return Results.Problem("Internal server error", statusCode: 500);
            }
        })
        .WithName("ExistsAccessRequestsByRequestCode")
        .WithSummary("Check if an access request exists by request code")
        .WithDescription("Returns whether an access request exists for the given request code.")
        .WithTags("AccessRequests")
        .WithOpenApi();

        return app;
    }
}
