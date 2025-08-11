using FairShare.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FairShare.Infrastructure.Configurations;

public class GroupsConfig : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Groups");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
        builder.Property(g => g.Description).IsRequired().HasMaxLength(500);
        builder.Property(g => g.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

        builder.HasOne(builder => builder.User)
               .WithMany(builder => builder.Groups)
               .HasForeignKey(builder => builder.CreatedByUserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
