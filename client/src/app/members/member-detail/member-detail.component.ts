import { Component, OnInit, ViewChild } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { MessageService } from '../../_services/message.service';
import { Message } from '../../_models/message';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs', {static: true}) memberTabs!: TabsetComponent;
  member!: Member;
  galleryOptions!: NgxGalleryOptions[];
  galleryImages!: NgxGalleryImage[];
  messages: Message[] = [];
  activeTab!: TabDirective;
  constructor(private memberService: MembersService, 
              private route: ActivatedRoute,
              private messageService: MessageService) { }

  ngOnInit() {
    this.route.data.subscribe(data =>{
      this.member = data['member'];
    })

    this.route.queryParams.subscribe(params =>{
      params['tab'] ? this.selectTab(params['tab']) : this.selectTab(0);
    })

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]
    this.galleryImages = this.getImages()
  }

  getImages(): NgxGalleryImage[] {
    const imageUrls = [];
    for (let photo of this.member.photos) {

      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url
      })
    }
    return imageUrls;
  }

  
  loadMessages() {
    this.messageService.getMessageThread(this.member.username).subscribe(res => {
      this.messages = res;
    })
  }

  selectTab(tabId: number){
    this.memberTabs.tabs[tabId].active = true;
  }

  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if(this.activeTab.heading === 'Messages' && this.messages.length === 0){
      this.loadMessages();
    }
  }
}
