using Application.AccessRequests.Commands;
using Application.AccessRequests.Queries;
using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;

namespace Application.Authorization.Commands;

public class VerifyAccessCommandHandler : ICommandHandler<VerifyAccessCommand, Result<AuthorizationResult>>
{
    private readonly ICommandDispatcher _commandDispatcher;

    public VerifyAccessCommandHandler(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    public async Task<Result<AuthorizationResult>> Handle(VerifyAccessCommand command, CancellationToken cancellation)
    {
        var createAccessRequestCommand = new CreateAccessRequestCommand(
            Guid.NewGuid(),
            command.ProfileUserName,
            command.JobNumber,
            command.CycleNumber,
            command.ActivityCode ?? string.Empty,
            command.Workstation,
            command.ApplicationName,
            UtcCreatedAt: DateTime.UtcNow
        );

        var result = await _commandDispatcher.Dispatch<CreateAccessRequestCommand, Result<AccessRequest>>(createAccessRequestCommand, cancellation);

        if (!result.IsSuccess)
        {
            return Result<AuthorizationResult>.Failure(result.Error);
        }

        return Result<AuthorizationResult>.Success(new AuthorizationResult
        {
            IsAuthorized = true,
            Source = "Access Granted"
        });
    }
}

