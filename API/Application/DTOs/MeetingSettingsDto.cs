namespace API.Application.DTOs;

public class MeetingSettingsDto
{
    public int FirstSessionOccurrenceRequiredManagementMembersCount { get; set; }
    public int SecondSessionOccurrenceRequiredManagementMembersCount { get; set; }
    public int ThirdSessionOccurrenceRequiredManagementMembersCount { get; set; }
    public int FirstSessionOccurrenceRequiredMembersCount { get; set; }
    public int SecondSessionOccurrenceRequiredMembersCount { get; set; }
    public int ThirdSessionOccurrenceRequiredMembersCount { get; set; }
}

public class UpsertMeetingSettingsDto
{
    public int FirstSessionOccurrenceRequiredManagementMembersCount { get; set; }
    public int SecondSessionOccurrenceRequiredManagementMembersCount { get; set; }
    public int ThirdSessionOccurrenceRequiredManagementMembersCount { get; set; }
    public int FirstSessionOccurrenceRequiredMembersCount { get; set; }
    public int SecondSessionOccurrenceRequiredMembersCount { get; set; }
    public int ThirdSessionOccurrenceRequiredMembersCount { get; set; }
}
