namespace API.Domain.Entities;

public class AgendaItemType : BaseEntity
{
    public string Name { get; set; } = null!; // e.g. "Discussion", "Decision", "Information"
    public string Description { get; set; } = null!; // e.g. "Agenda items that require discussion among attendees."
}