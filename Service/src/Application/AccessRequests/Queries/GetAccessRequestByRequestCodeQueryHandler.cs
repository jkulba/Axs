using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;

namespace Application.AccessRequests.Queries;

public class GetAccessRequestByRequestCodeQueryHandler : IQueryHandler<GetAccessRequestByRequestCodeQuery, Result<AccessRequest>>
{
    private readonly IAccessRequestRepository _repository;

    public GetAccessRequestByRequestCodeQueryHandler(IAccessRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<AccessRequest>> Handle(GetAccessRequestByRequestCodeQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var accessRequest = await _repository.GetByRequestCodeAsync(query.RequestCode, cancellationToken);

            if (accessRequest == null)
            {
                return Result<AccessRequest>.Failure(AccessRequestErrors.AccessRequestByRequestCodeNotFound(query.RequestCode));
            }
            return Result<AccessRequest>.Success(accessRequest);
        }
        catch (Exception ex)
        {
            return Result<AccessRequest>.Failure(new Error(ex.Message, "An error occurred while handling the access requests query"));
        }
    }
}
