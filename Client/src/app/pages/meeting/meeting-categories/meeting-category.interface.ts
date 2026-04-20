export interface MeetingCategory {
  id: number;
  code: string;
  name: string;
  nameAr: string;
  description: string;
  descriptionAr: string;
}

export interface UpsertMeetingCategoryDto {
  code: string;
  name: string;
  nameAr: string;
  description: string;
  descriptionAr: string;
}
