using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;

namespace Application.Users.Commands;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Result<User>>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<User>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.Id, cancellationToken);
        if (user == null)
        {
            return Result<User>.Failure(UserErrors.UserByIdNotFound(command.Id));
        }

        user.GraphId = command.GraphId;
        user.DisplayName = command.DisplayName;
        user.PrincipalName = command.PrincipalName;
        user.IsEnabled = command.IsEnabled;
        user.UtcUpdatedAt = DateTime.UtcNow;
        user.UpdatedByNum = "HAL-9000";

        await _userRepository.UpdateAsync(user, cancellationToken);
        return Result<User>.Success(user);
    }
}