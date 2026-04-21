export interface AgendaItemType {
  id: number;
  name: string;
  description: string;
}

export interface UpsertAgendaItemTypeDto {
  name: string;
  description: string;
}