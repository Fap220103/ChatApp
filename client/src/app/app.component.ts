import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { PresenceService } from './_services/presence.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'client';
  constructor(private accountService: AccountService, private presence: PresenceService ) {}
  ngOnInit() {
    this.setCurrentUser();
  }
  setCurrentUser() {
    const userString = localStorage.getItem('user'); // Lấy giá trị từ localStorage
    if (!userString) return;
      const user: User = JSON.parse(userString);
      if(user){
        this.accountService.setCurrentUser(user);
        this.presence.createHubConnection(user);
      }
    
  

  }

}
