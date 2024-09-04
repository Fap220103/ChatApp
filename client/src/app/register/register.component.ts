import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { User } from '../models/user.model';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Output() cancelRegister = new EventEmitter<boolean>();
 
  constructor(private accountService: AccountService) { }

  ngOnInit() {
    
  }

  register(){
    this.accountService.register(this.model).subscribe({
      next: (res)=>{
       
        this.cancel();
      },
      error: (err)=>{
        console.log(err);
      }
    })
  }

  cancel(){
    this.cancelRegister.emit(false);
  }
}
