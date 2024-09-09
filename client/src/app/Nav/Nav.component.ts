import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  categoryList: string[] = [
    "matches",
    "lists",
    "messages"
  ];
  currentUser$!: Observable<User | null>;
  constructor(private accountService: AccountService,
            private router: Router,
            private toastr: ToastrService) { }
  
  ngOnInit() {
    this.currentUser$ = this.accountService.currentUser$;
  }

  login() {
    this.accountService.login(this.model)
      .subscribe({
        next: (res) => {
          this.router.navigate(['/members']);
          this.toastr.success("Đăng nhập thành công","Thành công")
        }
      })
  }

  logout() {
    this.accountService.logout();
    localStorage.removeItem('currentUser');
    this.router.navigate(['/']);
  }
 
}
