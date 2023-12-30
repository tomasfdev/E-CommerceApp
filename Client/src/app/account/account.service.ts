import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Address, User } from '../shared/Models/user';
import { ReplaySubject, map, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  apiUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable(); //observable

  constructor(private http: HttpClient, private router: Router) {}

  loadCurrentUser(token: string | null) {
    if (token === null) {
      this.currentUserSource.next(null); //observable = null
      return of(null);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http.get<User>(this.apiUrl + 'account', { headers }).pipe(
      map((response) => {
        if (response) {
          localStorage.setItem('token', response.token);
          this.currentUserSource.next(response);
          return response;
        } else {
          return null;
        }
      })
    );
  }

  login(values: any) {
    return this.http.post<User>(this.apiUrl + 'account/login', values).pipe(
      map((response) => {
        localStorage.setItem('token', response.token);
        this.currentUserSource.next(response); //guarda response/user(que vem da api) observable
      })
    );
  }

  register(values: any) {
    return this.http.post<User>(this.apiUrl + 'account/register', values).pipe(
      map((response) => {
        localStorage.setItem('token', response.token);
        this.currentUserSource.next(response); //guarda response/user(que vem da api) observable
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  CheckEmailExists(email: string) {
    return this.http.get<boolean>(
      this.apiUrl + 'account/emailexists?email=' + email
    );
  }

  getUserAddress() {
    return this.http.get<Address>(this.apiUrl + 'account/address');
  }

  updateUserAddress(address: Address) {
    return this.http.put(this.apiUrl + 'account/address', address);
  }
}
