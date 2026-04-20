namespace API.Domain.Entities;

public class MeetingSettings : SimpleBaseEntity
{
    public int FirstSessionOccurrenceRequiredManagementMembersCount { get; set; } = 1;
    public int SecondSessionOccurrenceRequiredManagementMembersCount { get; set; } = 1;
    public int ThirdSessionOccurrenceRequiredManagementMembersCount { get; set; } = 1;
    public int FirstSessionOccurrenceRequiredMembersCount { get; set; } = 1;
    public int SecondSessionOccurrenceRequiredMembersCount { get; set; } = 1;
    public int ThirdSessionOccurrenceRequiredMembersCount { get; set; } = 1;
}