import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BaseComponent } from '../../../../core/components/base-component/base-component';
import { MeetingSettingsService } from '../meeting-settings.service';
import { UpsertMeetingSettingsDto } from '../meeting-settings.interface';
import { ToastrService } from 'ngx-toastr';
import { MeetingCategory } from '../../meeting-categories/meeting-category.interface';
import { MeetingCategoriesService } from '../../meeting-categories/meeting-categories.service';
import { LangRouterLinkDirective } from '../../../../core/directives/lang-router-link.directive';
import { FormInput } from '../../../../shared/components/input/input';
@Component({
  selector: 'app-meeting-settings-manage',
  imports: [CommonModule, ReactiveFormsModule,LangRouterLinkDirective,FormInput],
  templateUrl: './meeting-settings-manage.html',
  styleUrl: './meeting-settings-manage.css',
})
export class MeetingSettingsManage extends BaseComponent implements OnInit {
  private fb = inject(FormBuilder);
  private meetingSettingsService = inject(MeetingSettingsService);
private meetingCategoriesService = inject(MeetingCategoriesService);

  loadingSessionOccurrences = false;
  loadingCategories = false;
  saving = false;
 categories: MeetingCategory[] = [];

  sessionOccurrenceForm : FormGroup = new FormGroup({}); 

  ngOnInit(): void {
    this.sessionOccurrenceFormInitialize();
    this.loadSessionOccurrences();
    this.loadCategories();
  }
sessionOccurrenceFormInitialize() {
 this.sessionOccurrenceForm = this.fb.nonNullable.group({
    firstSessionOccurrenceRequiredManagementMembersCount: [1, [Validators.required, Validators.min(1)]],
    secondSessionOccurrenceRequiredManagementMembersCount: [1, [Validators.required, Validators.min(1)]],
    thirdSessionOccurrenceRequiredManagementMembersCount: [1, [Validators.required, Validators.min(1)]],
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

  onSubmit(): void {
    if (this.sessionOccurrenceForm.invalid) {
      this.sessionOccurrenceForm.markAllAsTouched();
      return;
    }

    const payload: UpsertMeetingSettingsDto = this.sessionOccurrenceForm.getRawValue();
    this.saving = true;

    this.meetingSettingsService.update(payload).subscribe({
      next: (settings) => {
        this.sessionOccurrenceForm.patchValue(settings);
        this.saving = false;
        this.toastr.success(this.lang() === 'en' ? 'Settings updated successfully' : 'تم تحديث الإعدادات بنجاح');
      },
      error: () => {
        this.saving = false;
      },
    });
  }

    loadCategories(): void {
      this.loadingCategories = true;
      this.meetingCategoriesService.getAll().subscribe({
        next: (res) => {
          this.categories = res;
          this.loadingCategories = false;
        },
        error: () => {
          this.loadingCategories = false;
        },
      });
    }
  
    onDeleteCategory(category: MeetingCategory): void {
      const message =
        this.lang() === 'en'
          ? `Delete "${category.name}" category?`
          : `هل تريد حذف تصنيف "${category.nameAr}"؟`;
  
      if (!confirm(message)) {
        return;
      }
  
      this.meetingCategoriesService.delete(category.id).subscribe({
        next: () => {
          this.categories = this.categories.filter((item) => item.id !== category.id);
        },
      });
    }
}
