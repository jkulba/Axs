using Application.Common;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.AccessRequests.Commands;

public class CreateAccessRequestCommandHandler : ICommandHandler<CreateAccessRequestCommand, Result<AccessRequest>>
{
    private readonly IAccessRequestRepository _repository;
    public CreateAccessRequestCommandHandler(IAccessRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<AccessRequest>> Handle(CreateAccessRequestCommand command, CancellationToken cancellation)
    {
        try
        {
            // Generate RequestCode if not provided or empty
            var requestCode = command.RequestCode ?? Guid.NewGuid();
            if (requestCode == Guid.Empty)
                requestCode = Guid.NewGuid();

            // Set UtcCreatedAt to current UTC time if not provided
            var utcCreatedAt = command.UtcCreatedAt ?? DateTime.UtcNow;

            // Parse JobNumber to int (validation ensures it's valid)
            // if (!int.TryParse(command.JobNumber, out int jobNumber))
            // {
            //     return Result<AccessRequest>.Failure(new Error("Validation.JobNumber", "Invalid job number format"));
            // }

            var accessRequest = new AccessRequest
            {
                RequestCode = requestCode,
                UserName = command.UserName!,
                JobNumber = command.JobNumber,
                CycleNumber = command.CycleNumber,
                ActivityCode = command.ActivityCode,
                ApplicationName = command.ApplicationName!,
                Workstation = command.Workstation,
                UtcCreatedAt = utcCreatedAt
            };

            var createdRequest = await _repository.AddAsync(accessRequest, cancellation);
            return Result<AccessRequest>.Success(createdRequest);
        }
        catch (Exception ex)
        {
            return Result<AccessRequest>.Failure(new Error("Unexpected.CreateError", $"Failed to create access request: {ex.Message}"));
        }
    }
}