using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccessRequestsController : ControllerBase
{
    private readonly IAccessRequestRepository _repository;
    private readonly ILogger<AccessRequestsController> _logger;

    public AccessRequestsController(
        IAccessRequestRepository repository,
        ILogger<AccessRequestsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccessRequest>>> GetAll()
    {
        try
        {
            var requests = await _repository.GetAllAsync();
            return Ok(requests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all access requests");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccessRequest>> GetById(int id)
    {
        try
        {
            var request = await _repository.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving access request with ID {RequestId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("by-request-code/{requestCode}")]
    public async Task<ActionResult<AccessRequest>> GetByRequestCode(Guid requestCode)
    {
        try
        {
            var request = await _repository.GetByRequestCodeAsync(requestCode);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving access request with code {RequestCode}", requestCode);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("by-job-number/{jobNumber}")]
    public async Task<ActionResult<IEnumerable<AccessRequest>>> GetByJobNumber(int jobNumber)
    {
        try
        {
            var requests = await _repository.GetByJobNumberAsync(jobNumber);
            return Ok(requests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving access requests for job number {JobNumber}", jobNumber);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("by-user/{userName}")]
    public async Task<ActionResult<IEnumerable<AccessRequest>>> GetByUserName(string userName)
    {
        try
        {
            var requests = await _repository.GetByUserNameAsync(userName);
            return Ok(requests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving access requests for user {UserName}", userName);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<AccessRequest>> Create(AccessRequest request)
    {
        try
        {
            if (request.RequestCode == Guid.Empty)
            {
                request.RequestCode = Guid.NewGuid();
            }

            if (request.UtcCreatedAt == null)
            {
                request.UtcCreatedAt = DateTime.UtcNow;
            }

            var createdRequest = await _repository.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdRequest.RequestId }, createdRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating access request");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AccessRequest>> Update(int id, AccessRequest request)
    {
        try
        {
            if (id != request.RequestId)
            {
                return BadRequest("ID in URL does not match ID in request body");
            }

            if (!await _repository.ExistsAsync(id))
            {
                return NotFound();
            }

            var updatedRequest = await _repository.UpdateAsync(request);
            return Ok(updatedRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating access request with ID {RequestId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            if (!await _repository.ExistsAsync(id))
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting access request with ID {RequestId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("exists/{requestCode}")]
    public async Task<ActionResult<bool>> ExistsByRequestCode(Guid requestCode)
    {
        try
        {
            var exists = await _repository.ExistsByRequestCodeAsync(requestCode);
            return Ok(exists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if access request exists with code {RequestCode}", requestCode);
            return StatusCode(500, "Internal server error");
        }
    }
}
