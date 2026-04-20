export interface MeetingCategory {
  id: number;
  name: string;
  description: string;
}

export interface UpsertMeetingCategoryDto {
  name: string;
  description: string;
}
