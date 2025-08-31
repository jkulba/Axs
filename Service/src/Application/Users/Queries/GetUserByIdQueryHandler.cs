using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;

namespace Application.Users.Queries;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, Result<User>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<User>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(query.Id, cancellationToken);
        
        if (user is null)
        {
            return Result<User>.Failure(UserErrors.UserByIdNotFound(query.Id));
        }

        return Result<User>.Success(user);
    }
}