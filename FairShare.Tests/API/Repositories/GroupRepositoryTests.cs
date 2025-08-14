using FairShare.Infrastructure.Data.Context;
using FairShare.Infrastructure.Data.Repositories;
using FairShare.Tests.Builders;
using Microsoft.EntityFrameworkCore;

namespace FairShare.Tests.API.Repositories;

public class GroupRepositoryTests
{
    private readonly FairShareDbContext _context;
    private readonly GroupRepository _groupRepository;

    public GroupRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FairShareDbContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        _context = new FairShareDbContext(options);
        _groupRepository = new GroupRepository(_context);
    }

    [Fact]
    public async Task GetGroupById_ShouldReturnGroup_WhenGroupExists()
    {
        var group = GroupBuilder.CreateGroups()[0];
        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();

        var result = await _groupRepository.GetGroupById(group.Id);

        Assert.NotNull(result);
        Assert.Multiple(() =>
        {
            Assert.Equal(group.Id, result.Id);
            Assert.Equal(group.Name, result.Name);
            Assert.Equal(group.Description, result.Description);
            Assert.Equal(group.CreatedByUserId, result.CreatedByUserId);
        });
    }
}
