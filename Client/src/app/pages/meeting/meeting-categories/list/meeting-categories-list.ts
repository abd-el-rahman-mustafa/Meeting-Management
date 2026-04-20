import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BaseComponent } from '../../../../core/components/base-component/base-component';
import { LangRouterLinkDirective } from '../../../../core/directives/lang-router-link.directive';
import { MeetingCategory } from '../meeting-category.interface';
import { MeetingCategoriesService } from '../meeting-categories.service';

@Component({
  selector: 'app-meeting-categories-list',
  imports: [CommonModule, RouterLink, LangRouterLinkDirective],
  templateUrl: './meeting-categories-list.html',
  styleUrl: './meeting-categories-list.css',
})
export class MeetingCategoriesList extends BaseComponent implements OnInit {
  private meetingCategoriesService = inject(MeetingCategoriesService);

  categories: MeetingCategory[] = [];
  loading = false;

  ngOnInit(): void {
    this.loadCategories();
  }

  get title(): string {
    return this.lang() === 'en' ? 'Meeting Categories' : 'تصنيفات الاجتماعات';
  }

  get subtitle(): string {
    return this.lang() === 'en' ? 'Manage available meeting categories' : 'إدارة تصنيفات الاجتماعات المتاحة';
  }

  get addLabel(): string {
    return this.lang() === 'en' ? 'Add Category' : 'إضافة تصنيف';
  }

  loadCategories(): void {
    this.loading = true;
    this.meetingCategoriesService.getAll().subscribe({
      next: (res) => {
        this.categories = res;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      },
    });
  }

  onDelete(category: MeetingCategory): void {
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
