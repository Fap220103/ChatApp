import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

@Component({
  selector: 'app-nav',
  templateUrl: './Nav.component.html',
  styleUrls: ['./Nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}

  currentUser$!: Observable<User | null>;
  constructor(private accountService: AccountService) { }
  currentUser!: User;
  ngOnInit() {
    this.currentUser$ = this.accountService.currentUser$;
    const userJson = localStorage.getItem('user'); // Lấy dữ liệu từ localStorage
    if (userJson) {
      this.currentUser = JSON.parse(userJson) as User; // Chuyển đổi từ JSON sang User
    }
  }

  login() {
    this.accountService.login(this.model)
      .subscribe({
        next: (res) => {
          localStorage.setItem('currentUser',JSON.stringify(res));
          console.log(res);
        },
        error: (err) => {
          console.log("error");
          console.log(err);
        }
      })
  }

  logout() {
    this.accountService.logout();
    localStorage.removeItem('currentUser');
  }
 
}
