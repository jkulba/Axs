using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccessRequestHistoryController : ControllerBase
{
    private readonly IAccessRequestHistoryRepository _accessRequestHistoryRepository;

    public AccessRequestHistoryController(IAccessRequestHistoryRepository accessRequestHistoryRepository)
    {
        _accessRequestHistoryRepository = accessRequestHistoryRepository ?? throw new ArgumentNullException(nameof(accessRequestHistoryRepository));
    }

    /// <summary>
    /// Get all access request history records
    /// </summary>
    /// <returns>List of access request history records</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccessRequestHistory>>> GetAccessRequestHistory()
    {
        var history = await _accessRequestHistoryRepository.GetAllAsync();
        return Ok(history);
    }

    /// <summary>
    /// Get access request history by ID
    /// </summary>
    /// <param name="id">History record ID</param>
    /// <returns>Access request history record</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<AccessRequestHistory>> GetAccessRequestHistory(int id)
    {
        var history = await _accessRequestHistoryRepository.GetByIdAsync(id);
        
        if (history == null)
        {
            return NotFound($"Access request history with ID {id} not found.");
        }

        return Ok(history);
    }

    /// <summary>
    /// Get access request history by request code
    /// </summary>
    /// <param name="requestCode">Request code</param>
    /// <returns>Access request history record</returns>
    [HttpGet("by-request-code/{requestCode}")]
    public async Task<ActionResult<AccessRequestHistory>> GetAccessRequestHistoryByRequestCode(Guid requestCode)
    {
        var history = await _accessRequestHistoryRepository.GetByRequestCodeAsync(requestCode);
        
        if (history == null)
        {
            return NotFound($"Access request history with request code {requestCode} not found.");
        }

        return Ok(history);
    }

    /// <summary>
    /// Get access request history by request ID
    /// </summary>
    /// <param name="requestId">Request ID</param>
    /// <returns>List of access request history records for the request</returns>
    [HttpGet("by-request/{requestId}")]
    public async Task<ActionResult<IEnumerable<AccessRequestHistory>>> GetAccessRequestHistoryByRequestId(int requestId)
    {
        var history = await _accessRequestHistoryRepository.GetByRequestIdAsync(requestId);
        return Ok(history);
    }

    /// <summary>
    /// Get access request history by employee number
    /// </summary>
    /// <param name="employeeNum">Employee number</param>
    /// <returns>List of access request history records for the employee</returns>
    [HttpGet("by-employee/{employeeNum}")]
    public async Task<ActionResult<IEnumerable<AccessRequestHistory>>> GetAccessRequestHistoryByEmployee(string employeeNum)
    {
        var history = await _accessRequestHistoryRepository.GetByEmployeeNumAsync(employeeNum);
        return Ok(history);
    }

    /// <summary>
    /// Get access request history by operation type
    /// </summary>
    /// <param name="operation">Operation type</param>
    /// <returns>List of access request history records for the operation</returns>
    [HttpGet("by-operation/{operation}")]
    public async Task<ActionResult<IEnumerable<AccessRequestHistory>>> GetAccessRequestHistoryByOperation(int operation)
    {
        var history = await _accessRequestHistoryRepository.GetByOperationAsync(operation);
        return Ok(history);
    }

    /// <summary>
    /// Get access request history by approval status
    /// </summary>
    /// <param name="approvalStatus">Approval status</param>
    /// <returns>List of access request history records with the approval status</returns>
    [HttpGet("by-approval-status/{approvalStatus}")]
    public async Task<ActionResult<IEnumerable<AccessRequestHistory>>> GetAccessRequestHistoryByApprovalStatus(int approvalStatus)
    {
        var history = await _accessRequestHistoryRepository.GetByApprovalStatusAsync(approvalStatus);
        return Ok(history);
    }

    /// <summary>
    /// Get access request history by job number
    /// </summary>
    /// <param name="jobNumber">Job number</param>
    /// <returns>List of access request history records for the job</returns>
    [HttpGet("by-job/{jobNumber}")]
    public async Task<ActionResult<IEnumerable<AccessRequestHistory>>> GetAccessRequestHistoryByJobNumber(int jobNumber)
    {
        var history = await _accessRequestHistoryRepository.GetByJobNumberAsync(jobNumber);
        return Ok(history);
    }

    /// <summary>
    /// Get access request history by approver
    /// </summary>
    /// <param name="approverNum">Approver number</param>
    /// <returns>List of access request history records approved by the approver</returns>
    [HttpGet("by-approver/{approverNum}")]
    public async Task<ActionResult<IEnumerable<AccessRequestHistory>>> GetAccessRequestHistoryByApprover(string approverNum)
    {
        var history = await _accessRequestHistoryRepository.GetByApproverAsync(approverNum);
        return Ok(history);
    }

    /// <summary>
    /// Get recent access request history
    /// </summary>
    /// <param name="days">Number of days to look back (default: 30)</param>
    /// <returns>List of recent access request history records</returns>
    [HttpGet("recent")]
    public async Task<ActionResult<IEnumerable<AccessRequestHistory>>> GetRecentAccessRequestHistory([FromQuery] int days = 30)
    {
        var history = await _accessRequestHistoryRepository.GetRecentHistoryAsync(days);
        return Ok(history);
    }

    /// <summary>
    /// Get access request history by date range
    /// </summary>
    /// <param name="fromDate">Start date</param>
    /// <param name="toDate">End date</param>
    /// <returns>List of access request history records within the date range</returns>
    [HttpGet("by-date-range")]
    public async Task<ActionResult<IEnumerable<AccessRequestHistory>>> GetAccessRequestHistoryByDateRange(
        [FromQuery] DateTime fromDate, 
        [FromQuery] DateTime toDate)
    {
        var history = await _accessRequestHistoryRepository.GetByDateRangeAsync(fromDate, toDate);
        return Ok(history);
    }

    /// <summary>
    /// Get expiring access requests
    /// </summary>
    /// <param name="beforeDate">Date before which access expires</param>
    /// <returns>List of access request history records with expiring access</returns>
    [HttpGet("expiring")]
    public async Task<ActionResult<IEnumerable<AccessRequestHistory>>> GetExpiringAccess([FromQuery] DateTime beforeDate)
    {
        var history = await _accessRequestHistoryRepository.GetExpiringAccessAsync(beforeDate);
        return Ok(history);
    }

    /// <summary>
    /// Create a new access request history record
    /// </summary>
    /// <param name="accessRequestHistory">Access request history to create</param>
    /// <returns>Created access request history record</returns>
    [HttpPost]
    public async Task<ActionResult<AccessRequestHistory>> CreateAccessRequestHistory(AccessRequestHistory accessRequestHistory)
    {
        if (accessRequestHistory == null)
        {
            return BadRequest("Access request history cannot be null.");
        }

        // Check if request code already exists
        if (await _accessRequestHistoryRepository.ExistsByRequestCodeAsync(accessRequestHistory.RequestCode))
        {
            return Conflict($"Access request history with request code {accessRequestHistory.RequestCode} already exists.");
        }

        var createdHistory = await _accessRequestHistoryRepository.AddAsync(accessRequestHistory);
        return CreatedAtAction(nameof(GetAccessRequestHistory), new { id = createdHistory.AccessRequestHistoryId }, createdHistory);
    }

    /// <summary>
    /// Update an existing access request history record
    /// </summary>
    /// <param name="id">History record ID</param>
    /// <param name="accessRequestHistory">Updated access request history</param>
    /// <returns>Updated access request history record</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<AccessRequestHistory>> UpdateAccessRequestHistory(int id, AccessRequestHistory accessRequestHistory)
    {
        if (accessRequestHistory == null)
        {
            return BadRequest("Access request history cannot be null.");
        }

        if (id != accessRequestHistory.AccessRequestHistoryId)
        {
            return BadRequest("History ID mismatch.");
        }

        if (!await _accessRequestHistoryRepository.ExistsAsync(id))
        {
            return NotFound($"Access request history with ID {id} not found.");
        }

        var updatedHistory = await _accessRequestHistoryRepository.UpdateAsync(accessRequestHistory);
        return Ok(updatedHistory);
    }

    /// <summary>
    /// Delete an access request history record
    /// </summary>
    /// <param name="id">History record ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccessRequestHistory(int id)
    {
        if (!await _accessRequestHistoryRepository.ExistsAsync(id))
        {
            return NotFound($"Access request history with ID {id} not found.");
        }

        await _accessRequestHistoryRepository.DeleteAsync(id);
        return NoContent();
    }
}
