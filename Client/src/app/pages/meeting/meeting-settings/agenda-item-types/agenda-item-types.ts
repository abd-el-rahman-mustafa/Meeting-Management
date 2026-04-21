import { Component, inject } from '@angular/core';
import { BaseComponent } from '../../../../core/components/base-component/base-component';
import { AgendaItemType } from './agenda-item-type.interface';
import { AgendaItemTypesService } from './agenda-item-types.service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormInput } from '../../../../shared/components/input/input';

@Component({
  selector: 'app-agenda-item-types',
  imports: [CommonModule, ReactiveFormsModule, FormInput],
  templateUrl: './agenda-item-types.html',
})
export class AgendaItemTypes extends BaseComponent {

private agendaItemTypesService = inject(AgendaItemTypesService);
private fb = inject(FormBuilder);

loadingTypes = false;
types: AgendaItemType[] = [];
editingTypeId: number | null = null;

newTypeForm = this.fb.nonNullable.group({
  name: ['', [Validators.required, Validators.maxLength(150)]],
  description: ['', [Validators.required, Validators.maxLength(500)]],
});

ngOnInit(): void {
  this.loadTypes();
}

///////////////////////////////////////////////////////////////////////////////////
// Agenda Item Types Management
///////////////////////////////////////////////////////////////////////////////////
addNewType(): void {
  const newType: AgendaItemType = { id: 0, name: '', description: '' };
  this.types.unshift(newType);
  this.editingTypeId = 0;
  this.newTypeForm.reset();
}

loadTypes(): void {
  this.loadingTypes = true;
  this.agendaItemTypesService.getAll().subscribe({
    next: (res) => {
      this.types = res;
      this.loadingTypes = false;
    },
    error: () => {
      this.loadingTypes = false;
    },
  });
}

updateType(type: AgendaItemType): void {
  this.editingTypeId = type.id;
  this.newTypeForm.patchValue({ name: type.name, description: type.description });
}

onDeleteType(type: AgendaItemType): void {
  const message =
    this.lang() === 'en'
      ? `Delete "${type.name}" type?`
      : `هل تريد حذف نوع "${type.name}"؟`;

  if (!confirm(message)) return;

  this.agendaItemTypesService.delete(type.id).subscribe({
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
    this.agendaItemTypesService.create(payload).subscribe({
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
    this.agendaItemTypesService.update(this.editingTypeId!, payload).subscribe({
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