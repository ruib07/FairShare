using FairShare.Application.Constants;
using FairShare.Application.Interfaces.Services;
using FairShare.Application.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FairShare.API.Controllers;

[Route($"api/{AppSettings.ApiVersion}/group-members")]
[Authorize]
public class GroupMembersController : ControllerBase
{
    private readonly IGroupMembersService _groupMembersService;

    public GroupMembersController(IGroupMembersService groupMembersService)
    {
        _groupMembersService = groupMembersService ?? 
            throw new ArgumentNullException(nameof(groupMembersService));
    }

    // GET api/v1/group-members/for-user/{userId}
    [HttpGet("for-user/{userId}")]
    public async Task<IActionResult> GetGroupsForUser(Guid userId)
    {
        return Ok(await _groupMembersService.GetGroupsForUser(userId));
    }

    // GET api/v1/group-members/in-group/{groupId}
    [HttpGet("in-group/{groupId}")]
    public async Task<IActionResult> GetUsersInGroup(Guid groupId)
    {
        return Ok(await _groupMembersService.GetUsersInGroup(groupId));
    }

    // GET api/v1/group-members/{groupId}/users/{userId}
    [HttpGet("{groupId}/users/{userId}")]
    public async Task<IActionResult> IsUserInGroup(Guid groupId, Guid userId)
    {
        return Ok(await _groupMembersService.IsUserInGroup(groupId, userId));
    }

    // POST api/v1/group-members
    [HttpPost]
    public async Task<IActionResult> AddUserToGroup([FromBody] AddUserToGroupRequestDTO request)
    {
        await _groupMembersService.AddUserToGroup(request.GroupId, request.UserId);

        return CreatedAtAction(nameof(GetUsersInGroup), new { groupId = request.GroupId }, null);
    }

    // DELETE api/v1/group-members/{groupId}/users/{userId}
    [HttpDelete("{groupId}/users/{userId}")]
    public async Task<IActionResult> RemoveUserFromGroup(Guid groupId, Guid userId)
    {
        await _groupMembersService.RemoveUserFromGroup(groupId, userId);

        return NoContent();
    }
}
