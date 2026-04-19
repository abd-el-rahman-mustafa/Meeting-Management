import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../env/env.dev';
import { MeetingCategory, UpsertMeetingCategoryDto } from './meeting-category.interface';
import { ApiResponse } from '../../core/interfaces/api.interface';

@Injectable({
  providedIn: 'root',
})
export class MeetingCategoriesService {
  private http = inject(HttpClient);
  private readonly url = `${environment.API_URL}meeting-categories`;

  getAll(): Observable<MeetingCategory[]> {
    return this.http
      .get<ApiResponse<MeetingCategory[]>>(this.url)
      .pipe(map((res) => res.data));
  }

  getById(id: number): Observable<MeetingCategory> {
    return this.http
      .get<ApiResponse<MeetingCategory>>(`${this.url}/${id}`)
      .pipe(map((res) => res.data));
  }

  create(payload: UpsertMeetingCategoryDto): Observable<MeetingCategory> {
    return this.http
      .post<ApiResponse<MeetingCategory>>(this.url, payload)
      .pipe(map((res) => res.data));
  }

  update(id: number, payload: UpsertMeetingCategoryDto): Observable<MeetingCategory> {
    return this.http
      .put<ApiResponse<MeetingCategory>>(`${this.url}/${id}`, payload)
      .pipe(map((res) => res.data));
  }

  delete(id: number): Observable<void> {
    return this.http
      .delete<ApiResponse<string>>(`${this.url}/${id}`)
      .pipe(map(() => void 0));
  }
}
