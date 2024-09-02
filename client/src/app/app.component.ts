import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { User } from './models/user.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'client';
  users$!: Observable<User[]>;
  constructor(private http: HttpClient){

  }
  ngOnInit() {
    this.http.get<User[]>('https://localhost:5001/api/users')
    .pipe(res => of(res))
    .subscribe({
      next: (res)=>{
        console.log(res);
        this.users$ = res;
        
      },
      error: (err) =>{
        console.log(err);
      }
    });
  }
}
