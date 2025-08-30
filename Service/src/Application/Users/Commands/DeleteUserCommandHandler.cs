using Application.Interfaces;
using Domain.Common;
using Domain.Errors;

namespace Application.Users.Commands;

public class DeleteUserCommandHandler
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.Id, cancellationToken);
        if (user == null)
        {
            return Result.Failure(UserErrors.UserNotFound);
        }

        await _userRepository.DeleteAsync(user.Id, cancellationToken);
        return Result.Success();
    }
}