import { Component, inject } from '@angular/core';
import { expand } from 'rxjs';
import { BaseComponent } from '../../../../core/components/base-component/base-component';
import { MeetingType } from './meeting-type.interface';
import { MeetingTypesService } from './meeting-types.service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormInput } from '../../../../shared/components/input/input';

@Component({
  selector: 'app-meeting-types',
  imports: [CommonModule, ReactiveFormsModule, FormInput],
  templateUrl: './meeting-types.html',
})
export class MeetingTypes extends BaseComponent {

private meetingTypesService = inject(MeetingTypesService);
private fb = inject(FormBuilder);

loadingTypes = false;
types: MeetingType[] = [];
editingTypeId: number | null = null;

newTypeForm = this.fb.nonNullable.group({
  name: ['', [Validators.required, Validators.maxLength(150)]],
  description: ['', [Validators.required, Validators.maxLength(500)]],
});
  ngOnInit(): void {
  this.loadTypes();
}
///////////////////////////////////////////////////////////////////////////////////
// Meeting Types Management
///////////////////////////////////////////////////////////////////////////////////
addNewType(): void {
  const newType: MeetingType = { id: 0, name: '', description: '' };
  this.types.unshift(newType);
  this.editingTypeId = 0;
  this.newTypeForm.reset();
}

loadTypes(): void {
  this.loadingTypes = true;
  this.meetingTypesService.getAll().subscribe({
    next: (res) => {
      this.types = res;
      this.loadingTypes = false;
    },
    error: () => {
      this.loadingTypes = false;
    },
  });
}

updateType(type: MeetingType): void {
  this.editingTypeId = type.id;
  this.newTypeForm.patchValue({ name: type.name, description: type.description });
}

onDeleteType(type: MeetingType): void {
  const message =
    this.lang() === 'en'
      ? `Delete "${type.name}" type?`
      : `هل تريد حذف نوع "${type.name}"؟`;

  if (!confirm(message)) return;

  this.meetingTypesService.delete(type.id).subscribe({
    next: () => {
      this.types = this.types.filter((item) => item.id !== type.id);
      this.toastr.success(
        this.lang() === 'en' ? 'Type deleted successfully' : 'تم حذف النوع بنجاح',
      );
    },
  });
}

saveNewType(): void {
  if (this.newTypeForm.invalid) {
    this.newTypeForm.markAllAsTouched();
    return;
  }

  const payload = this.newTypeForm.getRawValue();
  const isNew = this.editingTypeId === 0;

  if (isNew) {
    this.meetingTypesService.create(payload).subscribe({
      next: (newType) => {
        const index = this.types.findIndex((t) => t.id === 0);
        if (index !== -1) this.types[index] = newType;
        this.editingTypeId = null;
        this.toastr.success(
          this.lang() === 'en' ? 'Type added successfully' : 'تم إضافة النوع بنجاح',
        );
      },
    });
  } else {
    this.meetingTypesService.update(this.editingTypeId!, payload).subscribe({
      next: (updatedType) => {
        const index = this.types.findIndex((t) => t.id === this.editingTypeId);
        if (index !== -1) this.types[index] = updatedType;
        this.editingTypeId = null;
        this.toastr.success(
          this.lang() === 'en' ? 'Type updated successfully' : 'تم تحديث النوع بنجاح',
        );
      },
    });
  }
}

cancelNewType(): void {
  if (this.editingTypeId === 0) {
    this.types = this.types.filter((t) => t.id !== 0);
  }
  this.editingTypeId = null;
}
}
