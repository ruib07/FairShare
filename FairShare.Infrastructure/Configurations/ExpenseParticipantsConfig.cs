using FairShare.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FairShare.Infrastructure.Configurations;

public class ExpenseParticipantsConfig : IEntityTypeConfiguration<ExpenseParticipant>
{
    public void Configure(EntityTypeBuilder<ExpenseParticipant> builder)
    {
        builder.ToTable("ExpenseParticipants");
        builder.HasKey(ep => new { ep.ExpenseId, ep.UserId });

        builder.HasOne(ep => ep.Expense)
            .WithMany(e => e.ExpenseParticipants)
            .HasForeignKey(ep => ep.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ep => ep.User)
            .WithMany(u => u.ExpenseParticipants)
            .HasForeignKey(ep => ep.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ep => ep.ShareAmount).IsRequired().HasColumnType("decimal(10,2)");
    }
}
