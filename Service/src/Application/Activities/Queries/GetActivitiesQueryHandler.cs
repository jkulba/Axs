using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;
using Microsoft.Extensions.Logging;

namespace Application.Activities.Queries;

public class GetActivitiesQueryHandler : IQueryHandler<GetActivitiesQuery, Result<IEnumerable<Activity>>>
{
    private readonly IActivityRepository _repository;
    private readonly ILogger<GetActivitiesQueryHandler> _logger;

    public GetActivitiesQueryHandler(IActivityRepository repository, ILogger<GetActivitiesQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<Activity>>> Handle(GetActivitiesQuery query, CancellationToken cancellation)
    {
        try
        {
            var results = await _repository.GetAllAsync(cancellation);

            if (results == null || !results.Any())
            {
                _logger.LogWarning("No activities found");
                return Result<IEnumerable<Activity>>.Failure(ActivityErrors.ActivityNotFound);
            }
            return Result<IEnumerable<Activity>>.Success(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling GetActivitiesQuery");
            return Result<IEnumerable<Activity>>.Failure(new Error(ex.Message, "An error occurred while handling the activities query"));
        }
    }
}