using Application.Interfaces;
using Domain.Common;
using Domain.Entities;

namespace Application.Users.Commands;

public class CreateUserCommandHandler
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<User>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = new User
        {
            GraphId = command.GraphId,
            UserId = command.UserId,
            DisplayName = command.DisplayName,
            PrincipalName = command.PrincipalName,
            IsEnabled = command.IsEnabled
        };

        await _userRepository.AddAsync(user, cancellationToken);
        return Result<User>.Success(user);
    }
}