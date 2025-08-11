namespace FairShare.Domain.Models;

public class ExpenseParticipant
{
    public Guid ExpenseId { get; set; }
    public Expense Expense { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public decimal ShareAmount { get; set; }
}
