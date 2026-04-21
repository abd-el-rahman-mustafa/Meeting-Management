import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BaseComponent } from '../../../../core/components/base-component/base-component';
import { MeetingCategories } from '../meeting-categories/meeting-categories';
import { MeetingTypes } from '../meeting-types/meeting-types';
import { MeetingLevels } from '../meeting-levels/meeting-levels';
import { MeetingQuorum } from '../meeting-quorum/meeting-quorum';
@Component({
  selector: 'app-meeting-settings-manage',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MeetingCategories,
    MeetingTypes,
    MeetingLevels,
    MeetingQuorum,
  ],
  templateUrl: './meeting-settings-manage.html',
  styleUrl: './meeting-settings-manage.css',
})
export class MeetingSettingsManage extends BaseComponent implements OnInit {
  activeTab = 'quorum';

  tabs = [
    { key: 'quorum', labelEn: 'Quorum', labelAr: 'النصاب', icon: 'fa-solid fa-users-line' },
    { key: 'categories', labelEn: 'Categories', labelAr: 'التصنيفات', icon: 'fa-solid fa-tags' },
    { key: 'types', labelEn: 'Types', labelAr: 'الأنواع', icon: 'fa-solid fa-layer-group' },
    { key: 'levels', labelEn: 'Levels', labelAr: 'المستويات', icon: 'fa-solid fa-chart-bar' },
  ];
  
  ngOnInit(): void {
    this.setPageTitle(this.lang() === 'en' ? 'Meeting Settings' : 'إعدادات الاجتماع');
  }
}
