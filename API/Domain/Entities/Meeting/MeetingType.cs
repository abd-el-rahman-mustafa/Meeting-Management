namespace API.Domain.Entities;

public class MeetingType : BaseEntity
{
    public string Name { get; set; } = null!; // e.g. "Team Meeting", "Client Meeting", "Project Update"
}