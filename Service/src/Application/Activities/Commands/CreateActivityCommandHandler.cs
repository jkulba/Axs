using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;

namespace Application.Activities.Commands;

public class CreateActivityCommandHandler : ICommandHandler<CreateActivityCommand, Result<Activity>>
{
    private readonly IActivityRepository _repository;

    public CreateActivityCommandHandler(IActivityRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Activity>> Handle(CreateActivityCommand command, CancellationToken cancellationToken)
    {
        var activity = new Activity
        {
            ActivityCode = command.ActivityCode,
            ActivityName = command.ActivityName,
            Description = command.Description,
            IsActive = command.IsActive
        };

        await _repository.AddAsync(activity);
        return Result<Activity>.Success(activity);
    }
}
