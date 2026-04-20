namespace API.Domain.Entities;

public class MeetingCategory : BaseEntity
{
    public string Name { get; set; } = null!; // e.g. "Strategic Meeting", "Operational Meeting", "Project Meeting"
    public string Description { get; set; } = null!; // e.g. "Meetings focused on strategic planning and decision-making."
}