import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { User } from './_models/user.model';
import { AccountService } from './_services/account.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'client';
  users$!: Observable<User[]>;
  constructor(private accountService: AccountService,
              ){

  } 
  ngOnInit() {
  
   this.setCurrentUser();
  }
  setCurrentUser(){
    const userString = localStorage.getItem('user'); // Lấy giá trị từ localStorage
    const user: User = userString ? JSON.parse(userString) : null; // Kiểm tra null trước khi parse
    this.accountService.setCurrentUser(user);

  }
  
}
