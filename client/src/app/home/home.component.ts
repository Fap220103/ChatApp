import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode : boolean = false;
  constructor() { }

  ngOnInit() {

  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  }

  
  cancelRegisterMode(event: boolean){
    this.registerMode = event;
  }
}
