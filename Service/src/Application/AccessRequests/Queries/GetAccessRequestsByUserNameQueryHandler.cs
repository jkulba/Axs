using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;
using Microsoft.Extensions.Logging;

namespace Application.AccessRequests.Queries;

public class GetAccessRequestsByUserNameQueryHandler : IQueryHandler<GetAccessRequestsByUserNameQuery, Result<IEnumerable<AccessRequest>>>
{
    private readonly IAccessRequestRepository _repository;
    private readonly ILogger<GetAccessRequestsByUserNameQueryHandler> _logger;

    public GetAccessRequestsByUserNameQueryHandler(IAccessRequestRepository repository, ILogger<GetAccessRequestsByUserNameQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<AccessRequest>>> Handle(GetAccessRequestsByUserNameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var results = await _repository.GetByUserNameAsync(request.UserName, cancellationToken);

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