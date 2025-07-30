using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccessRequestsController : ControllerBase
{
    private readonly IAccessRequestRepository _accessRequestRepository;
    private readonly ILogger<AccessRequestsController> _logger;

    public AccessRequestsController(
        IAccessRequestRepository accessRequestRepository,
        ILogger<AccessRequestsController> logger)
    {
        _accessRequestRepository = accessRequestRepository ?? throw new ArgumentNullException(nameof(accessRequestRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all access requests
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccessRequest>>> GetAllAsync()
    {
        try
        {
            var requests = await _accessRequestRepository.GetAllAsync();
            return Ok(requests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all access requests");
            return StatusCode(500, "An error occurred while retrieving access requests");
        }
    }

    /// <summary>
    /// Get access request by ID
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AccessRequest>> GetByIdAsync(int id)
    {
        try
        {
            var request = await _accessRequestRepository.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound($"Access request with ID {id} not found");
            }
            return Ok(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving access request with ID {RequestId}", id);
            return StatusCode(500, "An error occurred while retrieving the access request");
        }
    }

    /// <summary>
    /// Get access request by request code
    /// </summary>
    [HttpGet("by-code/{requestCode:guid}")]
    public async Task<ActionResult<AccessRequest>> GetByRequestCodeAsync(Guid requestCode)
    {
        try
        {
            var request = await _accessRequestRepository.GetByRequestCodeAsync(requestCode);
            if (request == null)
            {
                return NotFound($"Access request with code {requestCode} not found");
            }
            return Ok(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving access request with code {RequestCode}", requestCode);
            return StatusCode(500, "An error occurred while retrieving the access request");
        }
    }

    /// <summary>
    /// Get access request by employee number
    /// </summary>
    [HttpGet("by-employee/{employeeNum}")]
    public async Task<ActionResult<AccessRequest>> GetByEmployeeNumAsync(string employeeNum)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(employeeNum))
            {
                return BadRequest("Employee number cannot be empty");
            }

            var request = await _accessRequestRepository.GetByEmployeeNumAsync(employeeNum);
            if (request == null)
            {
                return NotFound($"Access request for employee {employeeNum} not found");
            }
            return Ok(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving access request for employee {EmployeeNum}", employeeNum);
            return StatusCode(500, "An error occurred while retrieving the access request");
        }
    }

    /// <summary>
    /// Get access requests by approval status
    /// </summary>
    [HttpGet("by-status/{approvalStatus:int}")]
    public async Task<ActionResult<IEnumerable<AccessRequest>>> GetByApprovalStatusAsync(int approvalStatus)
    {
        try
        {
            var requests = await _accessRequestRepository.GetByApprovalStatusAsync(approvalStatus);
            return Ok(requests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving access requests with status {ApprovalStatus}", approvalStatus);
            return StatusCode(500, "An error occurred while retrieving access requests");
        }
    }

    /// <summary>
    /// Create a new access request
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AccessRequest>> CreateAsync([FromBody] AccessRequest accessRequest)
    {
        try
        {
            if (accessRequest == null)
            {
                return BadRequest("Access request cannot be null");
            }

            // Generate a new request code if not provided
            if (accessRequest.RequestCode == Guid.Empty)
            {
                accessRequest.RequestCode = Guid.NewGuid();
            }

            // Set creation timestamp
            accessRequest.UtcCreatedAt = DateTime.UtcNow;

            var createdRequest = await _accessRequestRepository.AddAsync(accessRequest);
            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = createdRequest.RequestId },
                createdRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating access request");
            return StatusCode(500, "An error occurred while creating the access request");
        }
    }

    /// <summary>
    /// Update an existing access request
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<AccessRequest>> UpdateAsync(int id, [FromBody] AccessRequest accessRequest)
    {
        try
        {
            if (accessRequest == null)
            {
                return BadRequest("Access request cannot be null");
            }

            if (id != accessRequest.RequestId)
            {
                return BadRequest("ID mismatch between route and body");
            }

            var exists = await _accessRequestRepository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound($"Access request with ID {id} not found");
            }

            // Set update timestamp
            accessRequest.UtcUpdatedAt = DateTime.UtcNow;

            var updatedRequest = await _accessRequestRepository.UpdateAsync(accessRequest);
            return Ok(updatedRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating access request with ID {RequestId}", id);
            return StatusCode(500, "An error occurred while updating the access request");
        }
    }

    /// <summary>
    /// Delete an access request
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            var exists = await _accessRequestRepository.ExistsAsync(id);
            if (!exists)
            {
                return NotFound($"Access request with ID {id} not found");
            }

            await _accessRequestRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting access request with ID {RequestId}", id);
            return StatusCode(500, "An error occurred while deleting the access request");
        }
    }
}
