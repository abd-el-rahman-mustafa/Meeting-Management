import { CommonModule } from '@angular/common';
import { Component, OnInit, computed, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from '../../../../core/components/base-component/base-component';
import { MeetingCategoriesService } from '../meeting-categories.service';
import { UpsertMeetingCategoryDto } from '../meeting-category.interface';

@Component({
  selector: 'app-meeting-categories-manage',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './meeting-categories-manage.html',
  styleUrl: './meeting-categories-manage.css',
})
export class MeetingCategoriesManage extends BaseComponent implements OnInit {
  private fb = inject(FormBuilder);
  private meetingCategoriesService = inject(MeetingCategoriesService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  categoryId: number | null = null;
  loading = false;
  saving = false;

  form = this.fb.nonNullable.group({
    code: ['', [Validators.required, Validators.maxLength(50)]],
    name: ['', [Validators.required, Validators.maxLength(150)]],
    nameAr: ['', [Validators.required, Validators.maxLength(150)]],
    description: ['', [Validators.required, Validators.maxLength(500)]],
    descriptionAr: ['', [Validators.required, Validators.maxLength(500)]],
  });

  isEditMode = computed(() => this.categoryId !== null);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.categoryId = id ? Number(id) : null;

    if (this.categoryId !== null && !Number.isNaN(this.categoryId)) {
      this.loadForEdit(this.categoryId);
    }
  }

  get pageTitle(): string {
    if (this.isEditMode()) {
      return this.lang() === 'en' ? 'Update Meeting Category' : 'تعديل تصنيف اجتماع';
    }
    return this.lang() === 'en' ? 'Add Meeting Category' : 'إضافة تصنيف اجتماع';
  }

  get submitLabel(): string {
    if (this.saving) {
      return this.lang() === 'en' ? 'Saving...' : 'جاري الحفظ...';
    }
    if (this.isEditMode()) {
      return this.lang() === 'en' ? 'Update' : 'تحديث';
    }
    return this.lang() === 'en' ? 'Create' : 'إنشاء';
  }

  loadForEdit(id: number): void {
    this.loading = true;
    this.meetingCategoriesService.getById(id).subscribe({
      next: (category) => {
        this.form.patchValue({
          code: category.code,
          name: category.name,
          nameAr: category.nameAr,
          description: category.description,
          descriptionAr: category.descriptionAr,
        });
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

    const payload: UpsertMeetingCategoryDto = this.form.getRawValue();
    this.saving = true;

    if (this.isEditMode() && this.categoryId !== null) {
      this.meetingCategoriesService.update(this.categoryId, payload).subscribe({
        next: () => {
          this.navigateToList();
        },
        error: () => {
          this.saving = false;
        },
      });
      return;
    }

    this.meetingCategoriesService.create(payload).subscribe({
      next: () => {
        this.navigateToList();
      },
      error: () => {
        this.saving = false;
      },
    });
  }

  backToList(): void {
    this.navigateToList();
  }

  private navigateToList(): void {
    this.router.navigate([`/${this.lang()}/meeting-categories`]);
  }
}
