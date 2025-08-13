namespace FairShare.Application.Shared.DTOs;

public class AddUserToGroupRequestDTO
{
    public Guid GroupId { get; set; }
    public Guid UserId { get; set; }
}
