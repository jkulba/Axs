using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Errors;

namespace Application.Users.Commands;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Result<int>>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<int>> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.Id, cancellationToken);
        if (user == null)
        {
            return Result<int>.Failure(UserErrors.UserNotFound);
        }

        var deletedCount = await _userRepository.DeleteAsync(user.Id, cancellationToken);
        return Result<int>.Success(deletedCount);
    }
}