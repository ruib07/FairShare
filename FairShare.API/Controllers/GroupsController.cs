using FairShare.Application.Constants;
using FairShare.Application.Interfaces.Services;
using FairShare.Application.Shared.DTOs;
using FairShare.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairShare.API.Controllers;

[Route($"api/{AppSettings.ApiVersion}/groups")]
[Authorize]
public class GroupsController : ControllerBase
{
    private readonly IGroupsService _groupsService;

    public GroupsController(IGroupsService groupsService)
    {
        _groupsService = groupsService ?? throw new ArgumentNullException(nameof(groupsService));
    }

    // GET api/v1/groups/{groupId}
    [HttpGet("{groupId}")]
    public async Task<IActionResult> GetGroupById(Guid groupId)
    {
        var result = await _groupsService.GetGroupById(groupId);

        if (!result.IsSuccess) return StatusCode(result.Error.StatusCode, result.Error);

        return Ok(result.Data);
    }

    // GET api/v1/groups/created-by/{userId}
    [HttpGet("created-by/{userId}")]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroupsCreatedByUserId(Guid userId)
    {
        return Ok(await _groupsService.GetGroupsCreatedByUserId(userId));
    }

    // POST api/v1/groups
    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] Group group)
    {
        var result = await _groupsService.CreateGroup(group);

        if (!result.IsSuccess) return StatusCode(result.Error.StatusCode, result.Error);

        var response = new ResponsesDTO.Creation(result.Message, result.Data.Id);

        return CreatedAtAction(nameof(GetGroupById), new { groupId = result.Data.Id }, response);
    }

    // PUT api/v1/groups/{groupId}
    [HttpPut("{groupId}")]
    public async Task<IActionResult> UpdateGroup(Guid groupId, [FromBody] Group updateGroup)
    {
        var result = await _groupsService.UpdateGroup(groupId, updateGroup);

        if (!result.IsSuccess) return StatusCode(result.Error.StatusCode, result.Error);

        return Ok(result.Message);
    }

    // DELETE api/v1/groups/{groupId}
    [HttpDelete("{groupId}")]
    public async Task<IActionResult> DeleteGroup(Guid groupId)
    {
        await _groupsService.DeleteGroup(groupId);

        return NoContent();
    }
}
