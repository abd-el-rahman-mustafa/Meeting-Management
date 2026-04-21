namespace API.Domain.Entities;

public class AgendaItemType : BaseEntity
{
    public string Name { get; set; } = null!; // e.g. "Discussion", "Decision", "Information"
}