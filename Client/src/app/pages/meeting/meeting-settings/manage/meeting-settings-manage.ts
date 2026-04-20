import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BaseComponent } from '../../../../core/components/base-component/base-component';
import { MeetingSettingsService } from '../meeting-settings.service';
import { UpsertMeetingSettingsDto } from '../meeting-settings.interface';
import { ToastrService } from 'ngx-toastr';
import { MeetingCategory } from '../meeting-categories/meeting-category.interface';
import { MeetingCategoriesService } from '../meeting-categories/meeting-categories.service';
import { FormInput } from '../../../../shared/components/input/input';
import { MeetingTypes } from '../meeting-types/meeting-types';
@Component({
  selector: 'app-meeting-settings-manage',
  imports: [CommonModule, ReactiveFormsModule, FormInput,MeetingTypes],
  templateUrl: './meeting-settings-manage.html',
  styleUrl: './meeting-settings-manage.css',
})
export class MeetingSettingsManage extends BaseComponent implements OnInit {
  private fb = inject(FormBuilder);
  private meetingSettingsService = inject(MeetingSettingsService);
  private meetingCategoriesService = inject(MeetingCategoriesService);

  loadingSessionOccurrences = false;
  loadingCategories = false;
  savingSessionOccurrences = false;
  categories: MeetingCategory[] = [];
  editingCategoryId: number | null = null;

  newCategoryForm = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    description: ['', [Validators.required, Validators.maxLength(500)]],
  });

  sessionOccurrenceForm: FormGroup = new FormGroup({});

  ngOnInit(): void {
    this.sessionOccurrenceFormInitialize();
    this.loadSessionOccurrences();
    this.loadCategories();
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
  ///////////////////////////////////////////////////////////////////////////////////
  // Meeting Categories Management
  ///////////////////////////////////////////////////////////////////////////////////
  addNewCategory(): void {
    // ADD A NEW EDITABLE RAW TO THE TOP OF THE TABLE
    const newCategory: MeetingCategory = {
      id: 0, // Temporary ID for new category
      name: '',
      description: '',
    };
    this.categories.unshift(newCategory);
    this.editingCategoryId = 0;
    this.newCategoryForm.reset();
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

  updateCategory(category: MeetingCategory): void {
    this.editingCategoryId = category.id;
    this.newCategoryForm.patchValue({
      name: category.name,
      description: category.description,
    });
  }

  onDeleteCategory(category: MeetingCategory): void {
    const message =
      this.lang() === 'en'
        ? `Delete "${category.name}" category?`
        : `هل تريد حذف تصنيف "${category.name}"؟`;

    if (!confirm(message)) {
      return;
    }

    this.meetingCategoriesService.delete(category.id).subscribe({
      next: () => {
        this.categories = this.categories.filter((item) => item.id !== category.id);
        this.toastr.success(
          this.lang() === 'en' ? 'Category deleted successfully' : 'تم حذف التصنيف بنجاح',
        );
      },
    });
  }

  saveNewCategory(): void {
    if (this.newCategoryForm.invalid) {
      this.newCategoryForm.markAllAsTouched();
      return;
    }

    const payload = this.newCategoryForm.getRawValue();
    const isNew = this.editingCategoryId === 0;

    if (isNew) {
      this.meetingCategoriesService.create(payload).subscribe({
        next: (newCategory) => {
          const index = this.categories.findIndex((c) => c.id === 0);
          if (index !== -1) this.categories[index] = newCategory;
          this.editingCategoryId = null;
          this.toastr.success(
            this.lang() === 'en' ? 'Category added successfully' : 'تم إضافة التصنيف بنجاح',
          );
        },
      });
    } else {
      this.meetingCategoriesService.update(this.editingCategoryId!, payload).subscribe({
        next: (updatedCategory) => {
          const index = this.categories.findIndex((c) => c.id === this.editingCategoryId);
          if (index !== -1) this.categories[index] = updatedCategory;
          this.editingCategoryId = null;
          this.toastr.success(
            this.lang() === 'en' ? 'Category updated successfully' : 'تم تحديث التصنيف بنجاح',
          );
        },
      });
    }
  }

  cancelNewCategory(): void {
    // Only remove the temporary row when adding new; editing restores the display row automatically
    if (this.editingCategoryId === 0) {
      this.categories = this.categories.filter((c) => c.id !== 0);
    }
    this.editingCategoryId = null;
  }
}
