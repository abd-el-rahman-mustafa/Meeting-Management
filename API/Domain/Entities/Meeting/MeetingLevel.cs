namespace API.Domain.Entities;

public class MeetingLevel : BaseEntity
{
    public string Name { get; set; } = null!; // e.g. "High", "Medium", "Low"
}