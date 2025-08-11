using FairShare.Domain.Enums;

namespace FairShare.Domain.Models;

public class GroupMember
{
    public Guid GroupId { get; set; }
    public Group Group { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTime JoinedAt { get; set; }
    public MemberRoles Role { get; set; }
}
