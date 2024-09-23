import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Message } from '../../_models/message';
import { MessageService } from '../../_services/message.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm!: NgForm;
  @Input() messages: Message[] = [];
  @Input() username!: string;
  messageContent!: string;
  loading = false;
  arraymess: Message[] = [];
  constructor(public messageService: MessageService) { }

  ngOnInit() {
    this.messageService.messageThread$.subscribe(mess =>{
      this.arraymess = mess;
    });
    console.log(this.arraymess)
  }
 
  sendMessage(){
    this.loading=true;
    this.messageService.sendMessage(this.username, this.messageContent)
    .then(() => {
      this.messageForm?.reset();
    })
    .finally(() => this.loading = false);
  }
}
 