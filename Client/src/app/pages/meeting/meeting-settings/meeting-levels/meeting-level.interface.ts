export interface MeetingLevel {
  id: number;
  name: string;
  description: string;
}

export interface UpsertMeetingLevelDto {
  name: string;
  description: string;
}