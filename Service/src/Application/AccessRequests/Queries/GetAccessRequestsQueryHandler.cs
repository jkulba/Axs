using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;

namespace Application.AccessRequests.Queries;

public class GetAccessRequestsQueryHandler : IQueryHandler<GetAccessRequestsQuery, Result<IEnumerable<AccessRequest>>>
{
    private readonly IAccessRequestRepository _repository;

    public GetAccessRequestsQueryHandler(IAccessRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<AccessRequest>>> Handle(GetAccessRequestsQuery query, CancellationToken cancellation)
    {
        try
        {
            var results = await _repository.GetAllAsync(cancellation);

            if (results == null || !results.Any())
            {
                return Result<IEnumerable<AccessRequest>>.Failure(AccessRequestErrors.AccessRequestsNotFound);
            }
            return Result<IEnumerable<AccessRequest>>.Success(results);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AccessRequest>>.Failure(new Error(ex.Message, "An error occurred while handling the access requests query"));
        }

    }
}