import { Component, OnInit } from '@angular/core';
import { MembersService } from '../_services/members.service';
import { Member } from '../_models/member';
import { Pagination } from '../_models/pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members!: Member[] | undefined;
  predicate = 'liked';
  pageNumber = 1;
  pageSize = 5;
  pagination!: Pagination;
  constructor(private memberService: MembersService) { }

  ngOnInit() {
    this.loadLikes();
  }
  loadLikes(){
    this.memberService.getLikes(this.predicate,this.pageNumber,this.pageSize).subscribe(res =>{
      this.members = res.result;
      this.pagination = res.pagination;
    })
  }
  pageChanged(event: any){
    this.pageNumber = event.page;
    this.loadLikes();
  }
}
