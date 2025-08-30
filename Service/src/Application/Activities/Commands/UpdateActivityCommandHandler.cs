using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;

namespace Application.Activities.Commands;

public class UpdateActivityCommandHandler : ICommandHandler<UpdateActivityCommand, Result<Activity>>
{
    private readonly IActivityRepository _activityRepository;

    public UpdateActivityCommandHandler(IActivityRepository activityRepository)
    {
        _activityRepository = activityRepository;
    }

    public async Task<Result<Activity>> Handle(UpdateActivityCommand command, CancellationToken cancellationToken)
    {
        var activity = await _activityRepository.GetByIdAsync(command.Id, cancellationToken);
        if (activity == null)
        {
            return Result<Activity>.Failure(ActivityErrors.ActivityNotFound);
        }

        activity.ActivityName = command.ActivityName;
        activity.Description = command.Description;
        activity.IsActive = command.IsActive;

        await _activityRepository.UpdateAsync(activity);
        return Result<Activity>.Success(activity);
    }
}
