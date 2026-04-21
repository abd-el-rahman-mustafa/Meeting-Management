import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BaseComponent } from '../../../../core/components/base-component/base-component';
import { MeetingSettingsService } from '../meeting-settings.service';
import { UpsertMeetingSettingsDto } from '../meeting-settings.interface';
import { ToastrService } from 'ngx-toastr';
import { FormInput } from '../../../../shared/components/input/input';
import { MeetingCategories } from '../meeting-categories/meeting-categories';
import { MeetingTypes } from '../meeting-types/meeting-types';
import { MeetingLevels } from '../meeting-levels/meeting-levels';
@Component({
  selector: 'app-meeting-settings-manage',
  imports: [CommonModule, ReactiveFormsModule, FormInput, MeetingCategories, MeetingTypes, MeetingLevels],
  templateUrl: './meeting-settings-manage.html',
  styleUrl: './meeting-settings-manage.css',
})
export class MeetingSettingsManage extends BaseComponent implements OnInit {
  private fb = inject(FormBuilder);
  private meetingSettingsService = inject(MeetingSettingsService);

  loadingSessionOccurrences = false;
  
  savingSessionOccurrences = false;
  

  sessionOccurrenceForm: FormGroup = new FormGroup({});

  ngOnInit(): void {
    this.sessionOccurrenceFormInitialize();
    this.loadSessionOccurrences();
    this.setPageTitle(this.lang() === 'en' ? 'Meeting Settings' : 'إعدادات الاجتماع');
  }

  sessionOccurrenceFormInitialize() {
    this.sessionOccurrenceForm = this.fb.nonNullable.group({
      firstSessionOccurrenceRequiredManagementMembersCount: [
        1,
        [Validators.required, Validators.min(1)],
      ],
      secondSessionOccurrenceRequiredManagementMembersCount: [
        1,
        [Validators.required, Validators.min(1)],
      ],
      thirdSessionOccurrenceRequiredManagementMembersCount: [
        1,
        [Validators.required, Validators.min(1)],
      ],
      firstSessionOccurrenceRequiredMembersCount: [1, [Validators.required, Validators.min(1)]],
      secondSessionOccurrenceRequiredMembersCount: [1, [Validators.required, Validators.min(1)]],
      thirdSessionOccurrenceRequiredMembersCount: [1, [Validators.required, Validators.min(1)]],
    });
  }

  loadSessionOccurrences(): void {
    this.loadingSessionOccurrences = true;
    this.meetingSettingsService.getSessionOccurrences().subscribe({
      next: (settings) => {
        this.sessionOccurrenceForm.patchValue(settings);
        this.loadingSessionOccurrences = false;
      },
      error: () => {
        this.loadingSessionOccurrences = false;
      },
    });
  }

  onSubmitSessionOccurrenceForm(): void {
    if (this.sessionOccurrenceForm.invalid) {
      this.sessionOccurrenceForm.markAllAsTouched();
      return;
    }

    const payload: UpsertMeetingSettingsDto = this.sessionOccurrenceForm.getRawValue();
    this.savingSessionOccurrences = true;

    this.meetingSettingsService.update(payload).subscribe({
      next: (settings) => {
        this.sessionOccurrenceForm.patchValue(settings);
        this.savingSessionOccurrences = false;
        this.toastr.success(
          this.lang() === 'en'
            ? 'Quorum Settings updated successfully'
            : 'تم تحديث إعدادات النصاب بنجاح',
        );
      },
      error: () => {
        this.savingSessionOccurrences = false;
      },
    });
  }

}
