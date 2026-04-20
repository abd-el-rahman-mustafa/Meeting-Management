export interface MeetingType {
  id: number;
  name: string;
  description: string;
}

export interface UpsertMeetingTypeDto {
  name: string;
  description: string;
}