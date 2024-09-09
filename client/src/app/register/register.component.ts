import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { User } from '../models/user.model';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Output() cancelRegister = new EventEmitter<boolean>();
 
  constructor(private accountService: AccountService,
              private toastr: ToastrService) { }

  ngOnInit() {
    
  }

  register(){
    this.accountService.register(this.model).subscribe({
      next: (res)=>{   
        this.cancel();
        this.toastr.success("Đăng ký thành công","Thành công")
      },
      error: (err)=>{
        console.log(err);
        this.toastr.error(err.error,"Lỗi")
      }
    })
  }

  cancel(){
    this.cancelRegister.emit(false);
  }
}
