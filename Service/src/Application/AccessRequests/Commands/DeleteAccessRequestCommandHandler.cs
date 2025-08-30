using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;

namespace Application.AccessRequests.Commands;

public class DeleteAccessRequestCommandHandler : ICommandHandler<DeleteAccessRequestCommand, Result<int>>
{
    private readonly IAccessRequestRepository _repository;

    public DeleteAccessRequestCommandHandler(IAccessRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<int>> Handle(DeleteAccessRequestCommand command, CancellationToken cancellation)
    {
        var accessRequest = await _repository.GetByIdAsync(command.Id, cancellation);
        if (accessRequest == null)
        {
            return Result<int>.Failure(new Error("NotFound.AccessRequest", "Access request not found"));
        }

        var rowsAffected = await _repository.DeleteAsync(command.Id, cancellation);

        return Result<int>.Success(rowsAffected);
    }
}