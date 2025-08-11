using FairShare.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FairShare.Infrastructure.Configurations;

public class SettlementsConfig : IEntityTypeConfiguration<Settlement>
{
    public void Configure(EntityTypeBuilder<Settlement> builder)
    {
        builder.ToTable("Settlements");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();

        builder.HasOne(s => s.Group)
            .WithMany(u => u.Settlements)
            .HasForeignKey(s => s.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.FromUser)
           .WithMany(u => u.Credits)
           .HasForeignKey(s => s.FromUserId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.ToUser)
            .WithMany(u => u.Debts)
            .HasForeignKey(s => s.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(s => s.Amount).IsRequired().HasColumnType("decimal(10,2)");
        builder.Property(s => s.Date).IsRequired();
    }
}
