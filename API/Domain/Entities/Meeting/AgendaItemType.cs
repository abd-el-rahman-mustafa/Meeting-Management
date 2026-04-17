namespace API.Domain.Entities;

public class AgendaItemType : BaseEntity
{
    public string Code { get; set; } = null!; // e.g. "DISCUSSION", "DECISION", "INFORMATION"
    public string Name { get; set; } = null!; // e.g. "Discussion", "Decision", "Information"
    public string NameAr { get; set; } = null!; // e.g. "مناقشة", "قرار", "معلومات"
    public string Description { get; set; } = null!; // e.g. "Agenda items that require discussion among attendees."
    public string DescriptionAr { get; set; } = null!; // e.g. "بنود جدول الأعمال التي تتطلب مناقشة بين الحضور."
}