namespace API.Domain.Entities;

public class MeetingType : BaseEntity
{
    public string Name { get; set; } = null!; // e.g. "Team Meeting", "Client Meeting", "Project Update"
    public string Description { get; set; } = null!; // e.g. "Regular team meetings to discuss project progress and blockers."
}