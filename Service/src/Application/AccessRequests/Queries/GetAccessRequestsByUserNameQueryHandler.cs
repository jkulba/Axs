using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;

namespace Application.AccessRequests.Queries;

public class GetAccessRequestsByUserNameQueryHandler : IQueryHandler<GetAccessRequestsByUserNameQuery, Result<IEnumerable<AccessRequest>>>
{
    private readonly IAccessRequestRepository _repository;

    public GetAccessRequestsByUserNameQueryHandler(IAccessRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<AccessRequest>>> Handle(GetAccessRequestsByUserNameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var results = await _repository.GetByUserNameAsync(request.UserName, cancellationToken);

            if (results == null || !results.Any())
            {
                return Result<IEnumerable<AccessRequest>>.Failure(AccessRequestErrors.AccessRequestByUserNameNotFound(request.UserName));
            }
            return Result<IEnumerable<AccessRequest>>.Success(results);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AccessRequest>>.Failure(new Error(ex.Message, "An error occurred while handling the access requests query"));
        }

    }
}