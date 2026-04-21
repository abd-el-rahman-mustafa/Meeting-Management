import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../../env/env.dev';
import { ApiResponse } from '../../../../core/interfaces/api.interface';
import { AgendaItemType, UpsertAgendaItemTypeDto } from './agenda-item-type.interface';

@Injectable({
  providedIn: 'root',
})
export class AgendaItemTypesService {
  private http = inject(HttpClient);
  private readonly url = `${environment.API_URL}agenda-item-types`;

  getAll(): Observable<AgendaItemType[]> {
    return this.http
      .get<ApiResponse<AgendaItemType[]>>(this.url)
      .pipe(map((res) => res.data));
  }

  getById(id: number): Observable<AgendaItemType> {
    return this.http
      .get<ApiResponse<AgendaItemType>>(`${this.url}/${id}`)
      .pipe(map((res) => res.data));
  }

  create(payload: UpsertAgendaItemTypeDto): Observable<AgendaItemType> {
    return this.http
      .post<ApiResponse<AgendaItemType>>(this.url, payload)
      .pipe(map((res) => res.data));
  }

  update(id: number, payload: UpsertAgendaItemTypeDto): Observable<AgendaItemType> {
    return this.http
      .put<ApiResponse<AgendaItemType>>(`${this.url}/${id}`, payload)
      .pipe(map((res) => res.data));
  }

  delete(id: number): Observable<void> {
    return this.http
      .delete<ApiResponse<string>>(`${this.url}/${id}`)
      .pipe(map(() => void 0));
  }
}