using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;

namespace Application.AccessRequests.Queries;

public class SearchAccessRequestsQueryHandler : IQueryHandler<SearchAccessRequestsQuery, Result<IEnumerable<AccessRequest>>>
{
    private readonly IAccessRequestRepository _repository;

    public SearchAccessRequestsQueryHandler(IAccessRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<AccessRequest>>> Handle(SearchAccessRequestsQuery query, CancellationToken cancellation)
    {
        try
        {
            var results = await _repository.SearchAsync(query.SearchParameters, cancellation);

            // Return success even if no results are found - empty list is a valid search result
            return Result<IEnumerable<AccessRequest>>.Success(results ?? Enumerable.Empty<AccessRequest>());
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AccessRequest>>.Failure(new Error(ex.Message, "An error occurred while handling the search access requests query"));
        }
    }
}
