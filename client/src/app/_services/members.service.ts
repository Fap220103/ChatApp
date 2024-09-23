import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Member } from '../_models/member';
import { Observable, map, of, take } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';



@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache = new Map();
  user!: User;
  userParams!: UserParams;

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.subscribe(user => {
      if (user) {
        this.userParams = new UserParams(user);
        this.user = user;
      }
    })
  }

  getUserParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }

  getMembers(userParams: UserParams) {
    //console.log(this.user);
    const cacheKey = this.user.username + '-' + Object.values(userParams).join('-');
    //console.log(cacheKey);
    var response = this.memberCache.get(cacheKey);
    //console.log(this.memberCache);
    if (response) {
      return of(response);
    }
    let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize);
    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);
    return getPaginatedResult<Member[]>(this.baseUrl + 'users', params, this.http).pipe(
      map((response) => {
        this.memberCache.set(cacheKey, response);
        return response;
      })
    );
  }


  getMember(username: string) {
    //console.log(this.memberCache);
    const member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: Member) => member.username === username);
    if (member) {
      return of(member);
    }
    return this.http.get<Member>(this.baseUrl + 'users/' + username)
  }
  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  addLike(username: string) {
    return this.http.post(this.baseUrl + 'likes/' + username, {});
  }
  getLikes(predicate: string, pageNumber: number, pageSize: number) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('predicate', predicate);
    return getPaginatedResult<Member[]>(this.baseUrl + 'likes', params, this.http);
  }
}
