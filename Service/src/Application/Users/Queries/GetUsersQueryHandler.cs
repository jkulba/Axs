using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;

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
        try
        {
            var results = await _userRepository.GetAllAsync(cancellationToken);

            if (results == null || !results.Any())
            {
                return Result<IEnumerable<User>>.Failure(UserErrors.UsersNotFound);
            }
            return Result<IEnumerable<User>>.Success(results);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<User>>.Failure(new Error(ex.Message, "An error occurred while handling the users query"));
        }
    }
}