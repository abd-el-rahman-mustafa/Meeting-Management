import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../../env/env.dev';
import { ApiResponse } from '../../../../core/interfaces/api.interface';
import { MeetingLevel, UpsertMeetingLevelDto } from './meeting-level.interface';

@Injectable({
  providedIn: 'root',
})
export class MeetingLevelsService {
  private http = inject(HttpClient);
  private readonly url = `${environment.API_URL}meeting-levels`;

  getAll(): Observable<MeetingLevel[]> {
    return this.http
      .get<ApiResponse<MeetingLevel[]>>(this.url)
      .pipe(map((res) => res.data));
  }

  getById(id: number): Observable<MeetingLevel> {
    return this.http
      .get<ApiResponse<MeetingLevel>>(`${this.url}/${id}`)
      .pipe(map((res) => res.data));
  }

  create(payload: UpsertMeetingLevelDto): Observable<MeetingLevel> {
    return this.http
      .post<ApiResponse<MeetingLevel>>(this.url, payload)
      .pipe(map((res) => res.data));
  }

  update(id: number, payload: UpsertMeetingLevelDto): Observable<MeetingLevel> {
    return this.http
      .put<ApiResponse<MeetingLevel>>(`${this.url}/${id}`, payload)
      .pipe(map((res) => res.data));
  }

  delete(id: number): Observable<void> {
    return this.http
      .delete<ApiResponse<string>>(`${this.url}/${id}`)
      .pipe(map(() => void 0));
  }
}