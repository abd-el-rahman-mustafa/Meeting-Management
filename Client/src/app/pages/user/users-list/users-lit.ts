import { UserService } from '../user.service';
import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Gender, User } from '../user.interface';
import { BaseComponent } from '../../../core/components/base-component/base-component';

@Component({
  selector: 'app-users-list',
  imports: [CommonModule],
  templateUrl: './users-list.html',
  styleUrl: './users-list.css',
})
export class UsersList extends BaseComponent implements OnInit {

  private userService = inject(UserService);
Gender = Gender;
  users: User[] = [];

  ngOnInit() {
    this.getAllUsers();
    this.setPageTitle(this.lang() === 'en' ? 'Users List' : 'قائمة المستخدمين');
  }

  getAllUsers() {
    this.userService.getAllUsers().subscribe((res: any) => {
      this.users = res;
    });
  }
}