import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { MessageService } from '../../_services/message.service';
import { Message } from '../../_models/message';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { PresenceService } from '../../_services/presence.service';
import { AccountService } from '../../_services/account.service';
import { User } from '../../_models/user';
import { take } from 'rxjs';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static: true}) memberTabs!: TabsetComponent;
  member!: Member;
  galleryOptions!: NgxGalleryOptions[];
  galleryImages!: NgxGalleryImage[];
  messages: Message[] = [];
  activeTab!: TabDirective;
  user!: User;
  constructor(public presence: PresenceService, 
              private route: ActivatedRoute,
              private messageService: MessageService,
              private accountService: AccountService,
              private router: Router) { 
                this.accountService.currentUser$.pipe(take(1)).subscribe(user =>{
                  if(user){
                    this.user = user;
                  }
                })  
                this.router.routeReuseStrategy.shouldReuseRoute = () => false;
              }
 

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
    if(this.activeTab.heading === 'Messages' && this.user){
      console.log("user: " + this.user);
      console.log("other user: " + this.member.username);
      this.messageService.createHubConnection(this.user, this.member.username);
    }
    else{
      this.messageService.stopHubConnection();
    }
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }
}
