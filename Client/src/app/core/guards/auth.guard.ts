import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable, map } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private accountService: AccountService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
      return this.accountService.currentUser$.pipe(
        map(response => {
          if (response) return true;  //se tiver user object return true e pode avançar
          else {  //caso contrario redireciona para login 
            this.router.navigate(['/account/login'], {queryParams: {returnUrl: state.url}});
            return false
          }
        })
      );
    }
}
