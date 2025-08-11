namespace FairShare.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public byte[] ProfilePicture { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Group> Groups { get; set; }
    public ICollection<GroupMember> GroupMembers { get; set; }
    public ICollection<Expense> Expenses { get; set; }
    public ICollection<ExpenseParticipant> ExpenseParticipants { get; set; }
    public ICollection<Settlement> Credits { get; set; } 
    public ICollection<Settlement> Debts { get; set; }
}
