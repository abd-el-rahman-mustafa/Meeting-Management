import { Component, inject, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../core/components/base-component/base-component';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MeetingSettingsService } from '../meeting-settings.service';
import { UpsertMeetingSettingsDto } from '../meeting-settings.interface';
import { FormInput } from '../../../../shared/components/input/input';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-meeting-quorum',
   imports: [CommonModule, ReactiveFormsModule, FormInput],
  templateUrl: './meeting-quorum.html',
  styleUrl: './meeting-quorum.css',
})
export class MeetingQuorum extends BaseComponent implements OnInit {

  private fb = inject(FormBuilder);
  private meetingSettingsService = inject(MeetingSettingsService);

  loadingSessionOccurrences = false;
  
  savingSessionOccurrences = false;
  

  sessionOccurrenceForm: FormGroup = new FormGroup({});

 ngOnInit(): void {
    this.sessionOccurrenceFormInitialize();
    this.loadSessionOccurrences();
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
