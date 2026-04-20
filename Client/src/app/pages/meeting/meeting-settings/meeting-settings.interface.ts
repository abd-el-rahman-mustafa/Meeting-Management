export interface MeetingSettings {
  firstSessionOccurrenceRequiredManagementMembersCount: number;
  secondSessionOccurrenceRequiredManagementMembersCount: number;
  thirdSessionOccurrenceRequiredManagementMembersCount: number;
  firstSessionOccurrenceRequiredMembersCount: number;
  secondSessionOccurrenceRequiredMembersCount: number;
  thirdSessionOccurrenceRequiredMembersCount: number;
}

export interface UpsertMeetingSettingsDto extends MeetingSettings {}
