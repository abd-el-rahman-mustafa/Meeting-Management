import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { BaseComponent } from '../../../core/components/base-component/base-component';
import { MeetingSettingsService } from '../meeting-settings.service';
import { UpsertMeetingSettingsDto } from '../meeting-settings.interface';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-meeting-settings-manage',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './meeting-settings-manage.html',
  styleUrl: './meeting-settings-manage.css',
})
export class MeetingSettingsManage extends BaseComponent implements OnInit {
  private fb = inject(FormBuilder);
  private meetingSettingsService = inject(MeetingSettingsService);

  loading = false;
  saving = false;

  form = this.fb.nonNullable.group({
    firstSessionOccurrenceRequiredManagementMembersCount: [1, [Validators.required, Validators.min(1)]],
    secondSessionOccurrenceRequiredManagementMembersCount: [1, [Validators.required, Validators.min(1)]],
    thirdSessionOccurrenceRequiredManagementMembersCount: [1, [Validators.required, Validators.min(1)]],
    firstSessionOccurrenceRequiredMembersCount: [1, [Validators.required, Validators.min(1)]],
    secondSessionOccurrenceRequiredMembersCount: [1, [Validators.required, Validators.min(1)]],
    thirdSessionOccurrenceRequiredMembersCount: [1, [Validators.required, Validators.min(1)]],
  });

  ngOnInit(): void {
    this.loadSettings();
  }

  get pageTitle(): string {
    return this.lang() === 'en' ? 'Meeting Settings' : 'إعدادات الاجتماعات';
  }

  get submitLabel(): string {
    if (this.saving) {
      return this.lang() === 'en' ? 'Saving...' : 'جاري الحفظ...';
    }
    return this.lang() === 'en' ? 'Save Settings' : 'حفظ الإعدادات';
  }

  loadSettings(): void {
    this.loading = true;
    this.meetingSettingsService.get().subscribe({
      next: (settings) => {
        this.form.patchValue(settings);
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      },
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const payload: UpsertMeetingSettingsDto = this.form.getRawValue();
    this.saving = true;

    this.meetingSettingsService.update(payload).subscribe({
      next: (settings) => {
        this.form.patchValue(settings);
        this.saving = false;
        this.toastr.success(this.lang() === 'en' ? 'Settings updated successfully' : 'تم تحديث الإعدادات بنجاح');
      },
      error: () => {
        this.saving = false;
      },
    });
  }
}
