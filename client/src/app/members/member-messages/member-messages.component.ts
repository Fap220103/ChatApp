import { AfterViewInit, ChangeDetectionStrategy, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { Message } from '../../_models/message';
import { MessageService } from '../../_services/message.service';
import { NgForm } from '@angular/forms';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit, AfterViewInit {
  @ViewChild('messageForm') messageForm!: NgForm;
  @ViewChild('scrollMe') private myScrollContainer!: ElementRef;
  @Input() username!: string;
  messageContent!: string;
  loading = false;
  constructor(public messageService: MessageService) { }
  ngAfterViewInit(): void {
    this.scrollToBottom();
  }
  scrollToBottom(): void {
    try {
      this.myScrollContainer.nativeElement.scrollTop = this.myScrollContainer.nativeElement.scrollHeight;
    } catch (err) {
      console.error('Scroll failed', err);
    }
  }
  ngOnInit() {
  
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
 