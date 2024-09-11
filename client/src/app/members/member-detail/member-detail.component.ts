import { Component, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member!: Member;
  constructor(private memberService: MembersService,private route: ActivatedRoute) { }

  ngOnInit() {
    this.loadMember();
   
  }

  loadMember(){
    this.memberService.getMember(this.route.snapshot.paramMap.get('username') || '').subscribe(member =>{
       this.member = member
    
      })
  }
}
