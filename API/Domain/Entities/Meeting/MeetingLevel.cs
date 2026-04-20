namespace API.Domain.Entities;

public class MeetingLevel : BaseEntity
{
    public string Name { get; set; } = null!; // e.g. "High", "Medium", "Low"
    public string Description { get; set; } = null!; // e.g. "High priority meetings that require immediate attention."
}