using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;
using Microsoft.Extensions.Logging;

namespace Application.AccessRequests.Queries;

public class GetAccessRequestsQueryHandler : IQueryHandler<GetAccessRequestsQuery, Result<IEnumerable<AccessRequest>>>
{
    private readonly IAccessRequestRepository _repository;
    private readonly ILogger<GetAccessRequestsQueryHandler> _logger;

    public GetAccessRequestsQueryHandler(IAccessRequestRepository repository, ILogger<GetAccessRequestsQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<AccessRequest>>> Handle(GetAccessRequestsQuery query, CancellationToken cancellation)
    {
        try
        {
            var results = await _repository.GetAllAsync(cancellation);

            if (results == null || !results.Any())
            {
                _logger.LogWarning("No access requests found");
                return Result<IEnumerable<AccessRequest>>.Failure(AccessRequestErrors.AccessRequestsNotFound);
            }
            return Result<IEnumerable<AccessRequest>>.Success(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling GetAccessRequestsQuery");
            return Result<IEnumerable<AccessRequest>>.Failure(new Error(ex.Message, "An error occurred while handling the access requests query"));
        }

    }
}