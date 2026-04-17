namespace API.Domain.Entities;

public class MeetingType : BaseEntity
{
    public string Code { get; set; } = null!; // e.g. "TEAM_MEETING", "CLIENT_MEETING", "PROJECT_UPDATE"
    public string Name { get; set; } = null!; // e.g. "Team Meeting", "Client Meeting", "Project Update"
    public string NameAr { get; set; } = null!; // e.g. "اجتماع الفريق", "اجتماع العميل", "تحديث المشروع"
    public string Description { get; set; } = null!; // e.g. "Regular team meetings to discuss project progress and blockers."
    public string DescriptionAr { get; set; } = null!; // e.g. "اجتماعات الفريق المنتظمة لمناقشة تقدم المشروع والعقبات."
}