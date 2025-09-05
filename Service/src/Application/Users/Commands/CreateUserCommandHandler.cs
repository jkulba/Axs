using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;

namespace Application.Users.Commands;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<User>>
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
            GraphId = Guid.NewGuid().ToString(),
            UserId = command.UserId.ToUpperInvariant(),
            DisplayName = command.DisplayName,
            PrincipalName = command.PrincipalName,
            IsEnabled = command.IsEnabled,
            UtcCreatedAt = DateTime.UtcNow,
            CreatedByNum = "829468",
            UtcUpdatedAt = null,
            UpdatedByNum = null
        };

        await _userRepository.AddAsync(user, cancellationToken);
        return Result<User>.Success(user);
    }
}