using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;

namespace Application.Users.Queries;

public class GetUserByUserIdQueryHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserByUserIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<User>> Handle(GetUserByUserIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUserIdAsync(query.UserId, cancellationToken);

        if (user == null)
        {
            return Result<User>.Failure(UserErrors.UserNotFound);
        }

        return Result<User>.Success(user);
    }
}