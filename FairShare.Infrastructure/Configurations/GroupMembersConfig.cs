using FairShare.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FairShare.Infrastructure.Configurations;

public class GroupMembersConfig : IEntityTypeConfiguration<GroupMember>
{
    public void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        builder.ToTable("GroupMembers");
        builder.HasKey(gm => new { gm.GroupId, gm.UserId });
        
        builder.HasOne(builder => builder.Group)
            .WithMany(group => group.GroupMembers)
            .HasForeignKey(builder => builder.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(builder => builder.User)
           .WithMany(group => group.GroupMembers)
           .HasForeignKey(builder => builder.UserId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.Property(gm => gm.JoinedAt).IsRequired().HasDefaultValueSql("GETDATE()");
        builder.Property(gm => gm.Role).IsRequired();
    }
}
