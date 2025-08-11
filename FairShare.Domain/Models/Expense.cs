using FairShare.Domain.Enums;

namespace FairShare.Domain.Models;

public class Expense
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
    public Guid PaidByUserId { get; set; }
    public User User { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Categories Category { get; set; }
    public byte[] ReceiptImage { get; set; }
    public ICollection<ExpenseParticipant> ExpenseParticipants { get; set; }
}
