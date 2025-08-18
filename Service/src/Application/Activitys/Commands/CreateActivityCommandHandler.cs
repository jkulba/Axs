using System.Diagnostics;
using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;

namespace Application.Activitys.Commands;

public class CreateActivityCommandHandler : ICommandHandler<CreateActivityCommand, Result<Domain.Entities.Activity>>
{
    private readonly IActivityRepository _repository;

    public CreateActivityCommandHandler(IActivityRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Domain.Entities.Activity>> Handle(CreateActivityCommand command, CancellationToken cancellationToken)
    {
        var activity = new Domain.Entities.Activity
        {
            ActivityCode = command.ActivityCode,
            ActivityName = command.ActivityName,
            Description = command.Description,
            IsActive = command.IsActive
        };

        await _repository.AddAsync(activity);
        return Result<Domain.Entities.Activity>.Success(activity);
    }
}

