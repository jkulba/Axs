using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;

namespace Application.Users.Queries;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, Result<IEnumerable<User>>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<IEnumerable<User>>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return Result<IEnumerable<User>>.Success(users);
    }
}