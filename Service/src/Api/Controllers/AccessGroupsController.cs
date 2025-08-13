using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccessGroupsController : ControllerBase
{
    private readonly IAccessGroupRepository _accessGroupRepository;

    public AccessGroupsController(IAccessGroupRepository accessGroupRepository)
    {
        _accessGroupRepository = accessGroupRepository ?? throw new ArgumentNullException(nameof(accessGroupRepository));
    }

    /// <summary>
    /// Get all access groups
    /// </summary>
    /// <returns>List of access groups</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccessGroup>>> GetAccessGroups()
    {
        var groups = await _accessGroupRepository.GetAllAsync();
        return Ok(groups);
    }

    /// <summary>
    /// Get access group by ID
    /// </summary>
    /// <param name="id">Group ID</param>
    /// <returns>Access group</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<AccessGroup>> GetAccessGroup(int id)
    {
        var group = await _accessGroupRepository.GetByIdAsync(id);
        
        if (group == null)
        {
            return NotFound($"Access group with ID {id} not found.");
        }

        return Ok(group);
    }

    /// <summary>
    /// Get access group by group code
    /// </summary>
    /// <param name="groupCode">Group code</param>
    /// <returns>Access group</returns>
    [HttpGet("by-code/{groupCode}")]
    public async Task<ActionResult<AccessGroup>> GetAccessGroupByCode(Guid groupCode)
    {
        var group = await _accessGroupRepository.GetByGroupCodeAsync(groupCode);
        
        if (group == null)
        {
            return NotFound($"Access group with code {groupCode} not found.");
        }

        return Ok(group);
    }

    /// <summary>
    /// Get active access groups (non-expired)
    /// </summary>
    /// <returns>List of active access groups</returns>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<AccessGroup>>> GetActiveAccessGroups()
    {
        var groups = await _accessGroupRepository.GetActiveGroupsAsync();
        return Ok(groups);
    }

    /// <summary>
    /// Create a new access group
    /// </summary>
    /// <param name="accessGroup">Access group to create</param>
    /// <returns>Created access group</returns>
    [HttpPost]
    public async Task<ActionResult<AccessGroup>> CreateAccessGroup(AccessGroup accessGroup)
    {
        if (accessGroup == null)
        {
            return BadRequest("Access group cannot be null.");
        }

        // Generate a new GroupCode if not provided
        if (accessGroup.GroupCode == Guid.Empty)
        {
            accessGroup.GroupCode = Guid.NewGuid();
        }

        // Check if group code already exists
        if (await _accessGroupRepository.ExistsByGroupCodeAsync(accessGroup.GroupCode))
        {
            return Conflict($"Access group with code {accessGroup.GroupCode} already exists.");
        }

        // Check if group name already exists
        if (!string.IsNullOrWhiteSpace(accessGroup.GroupName) && 
            await _accessGroupRepository.ExistsByGroupNameAsync(accessGroup.GroupName))
        {
            return Conflict($"Access group with name '{accessGroup.GroupName}' already exists.");
        }

        var createdGroup = await _accessGroupRepository.AddAsync(accessGroup);
        return CreatedAtAction(nameof(GetAccessGroup), new { id = createdGroup.GroupId }, createdGroup);
    }

    /// <summary>
    /// Update an existing access group
    /// </summary>
    /// <param name="id">Group ID</param>
    /// <param name="accessGroup">Updated access group</param>
    /// <returns>Updated access group</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<AccessGroup>> UpdateAccessGroup(int id, AccessGroup accessGroup)
    {
        if (accessGroup == null)
        {
            return BadRequest("Access group cannot be null.");
        }

        if (id != accessGroup.GroupId)
        {
            return BadRequest("Group ID mismatch.");
        }

        if (!await _accessGroupRepository.ExistsAsync(id))
        {
            return NotFound($"Access group with ID {id} not found.");
        }

        var updatedGroup = await _accessGroupRepository.UpdateAsync(accessGroup);
        return Ok(updatedGroup);
    }

    /// <summary>
    /// Delete an access group
    /// </summary>
    /// <param name="id">Group ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccessGroup(int id)
    {
        if (!await _accessGroupRepository.ExistsAsync(id))
        {
            return NotFound($"Access group with ID {id} not found.");
        }

        await _accessGroupRepository.DeleteAsync(id);
        return NoContent();
    }
}
