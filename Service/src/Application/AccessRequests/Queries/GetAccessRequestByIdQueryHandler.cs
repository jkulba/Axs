using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;

namespace Application.AccessRequests.Queries;

public class GetAccessRequestByIdQueryHandler : IQueryHandler<GetAccessRequestByIdQuery, Result<AccessRequest>>
{
    private readonly IAccessRequestRepository _repository;

    public GetAccessRequestByIdQueryHandler(IAccessRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<AccessRequest>> Handle(GetAccessRequestByIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var accessRequest = await _repository.GetByIdAsync(query.Id, cancellationToken);

            if (accessRequest == null)
            {
                return Result<AccessRequest>.Failure(AccessRequestErrors.AccessRequestByIdNotFound(query.Id));
            }
            return Result<AccessRequest>.Success(accessRequest);
        }
        catch (Exception ex)
        {
            return Result<AccessRequest>.Failure(new Error(ex.Message, "An error occurred while handling the access requests query"));
        }
    }
}