namespace API.Domain.Entities;

public class MeetingCategory : BaseEntity
{
    public string Name { get; set; } = null!; // e.g. "Strategic Meeting", "Operational Meeting", "Project Meeting"
}