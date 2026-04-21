import { Component, inject } from '@angular/core';
import { BaseComponent } from '../../../../core/components/base-component/base-component';
import { MeetingLevel } from './meeting-level.interface';
import { MeetingLevelsService } from './meeting-levels.service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormInput } from '../../../../shared/components/input/input';

@Component({
  selector: 'app-meeting-levels',
  imports: [CommonModule, ReactiveFormsModule, FormInput],
  templateUrl: './meeting-levels.html',
})
export class MeetingLevels extends BaseComponent {

private meetingLevelsService = inject(MeetingLevelsService);
private fb = inject(FormBuilder);

loadingLevels = false;
levels: MeetingLevel[] = [];
editingLevelId: number | null = null;

newLevelForm = this.fb.nonNullable.group({
  name: ['', [Validators.required, Validators.maxLength(150)]],
  description: ['', [Validators.required, Validators.maxLength(500)]],
});

ngOnInit(): void {
  this.loadLevels();
}

///////////////////////////////////////////////////////////////////////////////////
// Meeting Levels Management
///////////////////////////////////////////////////////////////////////////////////
addNewLevel(): void {
  const newLevel: MeetingLevel = { id: 0, name: '', description: '' };
  this.levels.unshift(newLevel);
  this.editingLevelId = 0;
  this.newLevelForm.reset();
}

loadLevels(): void {
  this.loadingLevels = true;
  this.meetingLevelsService.getAll().subscribe({
    next: (res) => {
      this.levels = res;
      this.loadingLevels = false;
    },
    error: () => {
      this.loadingLevels = false;
    },
  });
}

updateLevel(level: MeetingLevel): void {
  this.editingLevelId = level.id;
  this.newLevelForm.patchValue({ name: level.name, description: level.description });
}

onDeleteLevel(level: MeetingLevel): void {
  const message =
    this.lang() === 'en'
      ? `Delete "${level.name}" level?`
      : `هل تريد حذف مستوى "${level.name}"؟`;

  if (!confirm(message)) return;

  this.meetingLevelsService.delete(level.id).subscribe({
    next: () => {
      this.levels = this.levels.filter((item) => item.id !== level.id);
      this.toastr.success(
        this.lang() === 'en' ? 'Level deleted successfully' : 'تم حذف المستوى بنجاح',
      );
    },
  });
}

saveNewLevel(): void {
  if (this.newLevelForm.invalid) {
    this.newLevelForm.markAllAsTouched();
    return;
  }

  const payload = this.newLevelForm.getRawValue();
  const isNew = this.editingLevelId === 0;

  if (isNew) {
    this.meetingLevelsService.create(payload).subscribe({
      next: (newLevel) => {
        const index = this.levels.findIndex((l) => l.id === 0);
        if (index !== -1) this.levels[index] = newLevel;
        this.editingLevelId = null;
        this.toastr.success(
          this.lang() === 'en' ? 'Level added successfully' : 'تم إضافة المستوى بنجاح',
        );
      },
    });
  } else {
    this.meetingLevelsService.update(this.editingLevelId!, payload).subscribe({
      next: (updatedLevel) => {
        const index = this.levels.findIndex((l) => l.id === this.editingLevelId);
        if (index !== -1) this.levels[index] = updatedLevel;
        this.editingLevelId = null;
        this.toastr.success(
          this.lang() === 'en' ? 'Level updated successfully' : 'تم تحديث المستوى بنجاح',
        );
      },
    });
  }
}

cancelNewLevel(): void {
  if (this.editingLevelId === 0) {
    this.levels = this.levels.filter((l) => l.id !== 0);
  }
  this.editingLevelId = null;
}
}