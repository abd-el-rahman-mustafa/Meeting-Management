export interface MeetingCategory {
  id: number;
  code: string;
  name: string;
  description: string;
}

export interface UpsertMeetingCategoryDto {
  code: string;
  name: string;
  description: string;
}
