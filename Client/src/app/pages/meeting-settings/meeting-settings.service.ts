import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../env/env.dev';
import { ApiResponse } from '../../core/interfaces/api.interface';
import { MeetingSettings, UpsertMeetingSettingsDto } from './meeting-settings.interface';

@Injectable({
  providedIn: 'root',
})
export class MeetingSettingsService {
  private http = inject(HttpClient);
  private readonly url = `${environment.API_URL}meeting-settings`;

  get(): Observable<MeetingSettings> {
    return this.http
      .get<ApiResponse<MeetingSettings>>(this.url)
      .pipe(map((res) => res.data));
  }

  update(payload: UpsertMeetingSettingsDto): Observable<MeetingSettings> {
    return this.http
      .put<ApiResponse<MeetingSettings>>(this.url, payload)
      .pipe(map((res) => res.data));
  }
}
