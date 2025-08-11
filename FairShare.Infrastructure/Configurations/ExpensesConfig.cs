using FairShare.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FairShare.Infrastructure.Configurations;

public class ExpensesConfig : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("Expenses");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        
        builder.HasOne(builder => builder.Group)
               .WithMany(group => group.Expenses)
               .HasForeignKey(builder => builder.GroupId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(builder => builder.User)
               .WithMany(group => group.Expenses)
               .HasForeignKey(builder => builder.PaidByUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.Description).IsRequired().HasMaxLength(255);
        builder.Property(e => e.Amount).IsRequired().HasColumnType("decimal(10,2)");
        builder.Property(e => e.Date).IsRequired();
        builder.Property(e => e.Category).IsRequired();
        builder.Property(e => e.ReceiptImage).IsRequired(false).HasColumnType("VARBINARY(MAX)");
    }
}
