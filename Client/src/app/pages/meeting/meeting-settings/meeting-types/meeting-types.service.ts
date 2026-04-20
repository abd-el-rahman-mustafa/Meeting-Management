import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../../env/env.dev';
import { ApiResponse } from '../../../../core/interfaces/api.interface';
import { MeetingType, UpsertMeetingTypeDto } from './meeting-type.interface';

@Injectable({
  providedIn: 'root',
})
export class MeetingTypesService {
  private http = inject(HttpClient);
  private readonly url = `${environment.API_URL}meeting-types`;

  getAll(): Observable<MeetingType[]> {
    return this.http
      .get<ApiResponse<MeetingType[]>>(this.url)
      .pipe(map((res) => res.data));
  }

  getById(id: number): Observable<MeetingType> {
    return this.http
      .get<ApiResponse<MeetingType>>(`${this.url}/${id}`)
      .pipe(map((res) => res.data));
  }

  create(payload: UpsertMeetingTypeDto): Observable<MeetingType> {
    return this.http
      .post<ApiResponse<MeetingType>>(this.url, payload)
      .pipe(map((res) => res.data));
  }

  update(id: number, payload: UpsertMeetingTypeDto): Observable<MeetingType> {
    return this.http
      .put<ApiResponse<MeetingType>>(`${this.url}/${id}`, payload)
      .pipe(map((res) => res.data));
  }

  delete(id: number): Observable<void> {
    return this.http
      .delete<ApiResponse<string>>(`${this.url}/${id}`)
      .pipe(map(() => void 0));
  }
}
