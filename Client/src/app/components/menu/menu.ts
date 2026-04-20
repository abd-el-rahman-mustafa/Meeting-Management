import { CommonModule } from '@angular/common';
import { Component, output, signal } from '@angular/core';
import {  RouterLinkActive } from '@angular/router';
import { BaseComponent } from '../../core/components/base-component/base-component';
import { LangRouterLinkDirective } from '../../core/directives/lang-router-link.directive';


interface NavItem {
  label: string;
  route: string;
  icon: string;
  badge?: number;
  adminOnly?: boolean;
}
@Component({
  selector: 'app-menu',
  imports: [LangRouterLinkDirective, RouterLinkActive, CommonModule],
  templateUrl: './menu.html',
  styleUrl: './menu.css',
})

export class MenuComponent extends BaseComponent {
  collapsed = signal(false);
  collapseChange = output<boolean>();
 
  navItems: NavItem[] = [
    {
      label: this.lang()== 'en'? 'Dashboard' : 'لوحة التحكم',
      route: '/dashboard',
      icon: 'fa-solid fa-table-cells-large',
    },
    {
      label: this.lang()== 'en'? 'Meetings' : 'الاجتماعات',
      route: '/meetings',
      icon: 'fa-solid fa-calendar',
    },
    {
      label: this.lang()== 'en'? 'Meeting Categories' : 'تصنيفات الاجتماعات',
      route: '/meeting-categories',
      icon: 'fa-solid fa-layer-group',
      adminOnly: true,
    },
    {
      label: this.lang()== 'en'? 'Meeting Settings' : 'إعدادات الاجتماعات',
      route: '/meeting-settings',
      icon: 'fa-solid fa-sliders',
      adminOnly: true,
    },
    {
      label: this.lang()== 'en'? 'Users' : 'المستخدمين',
      route: '/users',
      icon: 'fa-solid fa-users',
      adminOnly: true,
    },
    {
      label: this.lang()== 'en'? 'Settings' : 'الإعدادات',
      route: '/settings',
      icon: 'fa-solid fa-gear',
    },
  ];

  get visibleNavItems(): NavItem[] {
    return this.navItems.filter((item) => !item.adminOnly || this.isAdmin);
  }
 
  toggle(): void {
    this.collapsed.update(v => !v);
    this.collapseChange.emit(this.collapsed());
  }
}
