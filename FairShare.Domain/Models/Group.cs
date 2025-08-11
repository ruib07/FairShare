namespace FairShare.Domain.Models;

public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid CreatedByUserId { get; set; }
    public User User { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<GroupMember> GroupMembers { get; set; }
    public ICollection<Expense> Expenses { get; set; }
    public ICollection<Settlement> Settlements { get; set; }
}
