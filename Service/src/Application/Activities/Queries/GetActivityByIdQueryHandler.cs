using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;

namespace Application.Activities.Queries;

public class GetActivityByIdQueryHandler : IQueryHandler<GetActivityByIdQuery, Result<Activity>>
{
    private readonly IActivityRepository _activityRepository;

    public GetActivityByIdQueryHandler(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<Result<Activity>> Handle(GetActivityByIdQuery query, CancellationToken cancellationToken)
    {
        var activity = await _activityRepository.GetByIdAsync(query.ActivityId, cancellationToken);
        if (activity == null)
        {
            return Result<Activity>.Failure(ActivityErrors.ActivityNotFound);
        }

        return Result<Activity>.Success(activity);
    }
}