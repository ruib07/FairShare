using FairShare.Domain.Models;

namespace FairShare.Tests.Builders;

public class GroupBuilder
{
    private static int _counter = 2;

    public static List<Group> CreateGroups(int quantity = 2)
    {
        var groups = new List<Group>();

        for (int i = 0; i < quantity; i++)
        {
            groups.Add(new Group()
            {
                Id = Guid.NewGuid(),
                Name = $"Group {_counter}",
                Description = $"Description for group {_counter}",
                CreatedByUserId = Guid.NewGuid()
            });

            _counter++;
        }

        return groups;
    }
}
