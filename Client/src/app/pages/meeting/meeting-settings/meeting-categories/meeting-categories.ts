import { Component, inject } from '@angular/core';
import { BaseComponent } from '../../../../core/components/base-component/base-component';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormInput } from '../../../../shared/components/input/input';
import { MeetingCategory } from './meeting-category.interface';
import { MeetingCategoriesService } from './meeting-categories.service';

@Component({
  selector: 'app-meeting-categories',
  imports: [CommonModule, ReactiveFormsModule, FormInput],
  templateUrl: './meeting-categories.html',
})
export class MeetingCategories extends BaseComponent {

  private meetingCategoriesService = inject(MeetingCategoriesService);
  private fb = inject(FormBuilder);

  loadingCategories = false;
  categories: MeetingCategory[] = [];
  editingCategoryId: number | null = null;

  newCategoryForm = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    description: ['', [Validators.required, Validators.maxLength(500)]],
  });


  ngOnInit(): void {
    this.loadCategories();
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
