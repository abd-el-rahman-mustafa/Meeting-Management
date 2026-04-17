namespace API.Domain.Entities;

public class MeetingLevel : BaseEntity
{
    public string Code { get; set; } = null!; // e.g. "HIGH", "MEDIUM", "LOW"
    public string Name { get; set; } = null!; // e.g. "High", "Medium", "Low"
    public string NameAr { get; set; } = null!; // e.g. "عالي", "متوسط", "منخفض"
    public string Description { get; set; } = null!; // e.g. "High priority meetings that require immediate attention."
    public string DescriptionAr { get; set; } = null!; // e.g. "الاجتماعات ذات الأولوية العالية التي تتطلب اهتمامًا فوريًا."
}