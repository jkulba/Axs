using Application.Common;
using Application.Interfaces;
using Domain.Common;

namespace Application.AccessRequests.Queries;

public class ExistsAccessRequestByRequestCodeQueryHandler : IQueryHandler<ExistsAccessRequestByRequestCodeQuery, Result<bool>>
{
    private readonly IAccessRequestRepository _repository;

    public ExistsAccessRequestByRequestCodeQueryHandler(IAccessRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(ExistsAccessRequestByRequestCodeQuery query, CancellationToken cancellationToken)
    {
        if (query.RequestCode == Guid.Empty)
        {
            return Result<bool>.Failure(new Error("Validation.RequestCode", "Request code cannot be empty"));
        }

        var exists = await _repository.ExistsByRequestCodeAsync(query.RequestCode, cancellationToken);
        return Result<bool>.Success(exists);
    }
}