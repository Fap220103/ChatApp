import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Observable, map } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})

export class AdminGuard implements CanActivate {
  constructor(private accountService: AccountService,
              private toastr: ToastrService) { }
  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user => {
        if (user?.roles.includes('Admin') || user?.roles.includes('Moderator')) {
          return true;    
        }
        this.toastr.error('You cannot enter this area!'); // Show error message
        return false; 
      })
    );
  }
}
