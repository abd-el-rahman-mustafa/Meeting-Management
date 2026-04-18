namespace API.Domain.Entities;

public class MeetingCategory : BaseEntity
{
    public string Code { get; set; } = null!; // e.g. "STRATEGIC", "OPERATIONAL", "PROJECT"
    public string Name { get; set; } = null!; // e.g. "Strategic Meeting", "Operational Meeting", "Project Meeting"
    public string NameAr { get; set; } = null!; // e.g. "اجتماع استراتيجي", "اجتماع تشغيلي", "اجتماع مشروع"
    public string Description { get; set; } = null!; // e.g. "Meetings focused on strategic planning and decision-making."
    public string DescriptionAr { get; set; } = null!; // e.g. "الاجتماعات التي تركز على التخطيط الاستراتيجي واتخاذ القرار."
}