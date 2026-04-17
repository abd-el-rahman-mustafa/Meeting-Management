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
      label: this.lang()== 'en'? 'Users' : 'المستخدمين',
      route: '/users',
      icon: 'fa-solid fa-users',
    },
    {
      label: this.lang()== 'en'? 'Settings' : 'الإعدادات',
      route: '/settings',
      icon: 'fa-solid fa-gear',
    },
  ];
 
   toggle(): void {
    this.collapsed.update(v => !v);
    this.collapseChange.emit(this.collapsed());
  }
}
