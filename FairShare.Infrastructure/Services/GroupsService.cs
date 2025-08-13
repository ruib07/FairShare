using FairShare.Application.Interfaces.Repositories;
using FairShare.Application.Interfaces.Services;
using FairShare.Application.Shared.Common;
using FairShare.Domain.Models;

namespace FairShare.Infrastructure.Services;

public class GroupsService : IGroupsService
{
    private readonly IGroupRepository _groupRepository;

    public GroupsService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository ?? 
            throw new ArgumentNullException(nameof(groupRepository));
    }

    public async Task<Result<Group>> GetGroupById(Guid groupId)
    {
        var group = await _groupRepository.GetGroupById(groupId);

        if (group == null) return Result<Group>.Fail("Group not found.", 404);

        return Result<Group>.Success(group);
    }

    public async Task<IEnumerable<Group>> GetGroupsCreatedByUserId(Guid userId)
    {
        return await _groupRepository.GetGroupsCreatedByUserId(userId);
    }

    public async Task<Result<Group>> CreateGroup(Group group)
    {
        var validation = ValidateGroupFields(group);

        if (!validation.IsSuccess) 
            return Result<Group>.Fail(validation.Error.Message, validation.Error.StatusCode);

        var createdGroup = await _groupRepository.CreateGroup(group);

        return Result<Group>.Success(createdGroup, "Group created successfully.");
    }

    public async Task<Result<Group>> UpdateGroup(Guid groupId, Group updateGroup)
    {
        var currentGroup = await _groupRepository.GetGroupById(groupId);

        var validation = ValidateGroupFields(updateGroup);

        if (!validation.IsSuccess) 
            return Result<Group>.Fail(validation.Error.Message, validation.Error.StatusCode);

        currentGroup.Name = updateGroup.Name;
        currentGroup.Description = updateGroup.Description;

        await _groupRepository.UpdateGroup(currentGroup);

        return Result<Group>.Success(currentGroup, "Group updated successfully.");
    }

    public async Task DeleteGroup(Guid groupId)
    {
        await _groupRepository.DeleteGroup(groupId);
    }

    #region Private Methods

    private static Result<bool> ValidateGroupFields(Group group)
    {
        if (string.IsNullOrWhiteSpace(group.Name))
            return Result<bool>.Fail("Name is required.", 400);

        if (string.IsNullOrWhiteSpace(group.Description))
            return Result<bool>.Fail("Description is required.", 400);

        return Result<bool>.Success(true);
    }

    #endregion Private Methods
}
