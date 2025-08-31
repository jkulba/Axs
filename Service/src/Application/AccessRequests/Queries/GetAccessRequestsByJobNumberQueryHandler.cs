using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;

namespace Application.AccessRequests.Queries;

public class GetAccessRequestsByJobNumberQueryHandler : IQueryHandler<GetAccessRequestsByJobNumberQuery, Result<IEnumerable<AccessRequest>>>
{
    private readonly IAccessRequestRepository _repository;

    public GetAccessRequestsByJobNumberQueryHandler(IAccessRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<AccessRequest>>> Handle(GetAccessRequestsByJobNumberQuery query, CancellationToken cancellation)
    {
        try
        {
            var results = await _repository.GetByJobNumberAsync(query.JobNumber, cancellation);

            if (results == null || !results.Any())
            {
                return Result<IEnumerable<AccessRequest>>.Failure(AccessRequestErrors.AccessRequestByJobNumberNotFound(query.JobNumber));
            }
            return Result<IEnumerable<AccessRequest>>.Success(results);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AccessRequest>>.Failure(new Error(ex.Message, "An error occurred while handling the access requests query"));
        }
    }
}