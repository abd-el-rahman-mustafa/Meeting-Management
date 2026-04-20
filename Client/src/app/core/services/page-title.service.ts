import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PageTitleService {
  pageTitle = signal<string>('rent ms');

  setTitle(title: string) {
    this.pageTitle.set(title);
  }
}