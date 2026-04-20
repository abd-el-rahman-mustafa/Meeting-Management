import { Component, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, NavigationEnd } from '@angular/router';
import { filter, map } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';
import { LanguageService } from '../../core/services/language.service'; // adjust path as needed
import { BaseComponent } from '../../core/components/base-component/base-component';
import { LanguageBtn } from "../../shared/components/language-btn/language-btn";

import { UserMenuComponent } from '../../shared/components/user-menu/user-menu';
import { PageTitleService } from '../../core/services/page-title.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, LanguageBtn, UserMenuComponent],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class NavbarComponent extends BaseComponent {

  pageTitle = this.pageTitleService.pageTitle;
}