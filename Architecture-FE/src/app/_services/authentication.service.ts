import { User } from './../_models/user.model';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { config } from '../../config';
import { Token, Login, Register, EditUser } from '../_models';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private currentTokenSubject: BehaviorSubject<Token>;
  public currentToken: Observable<Token>;

  constructor(private http: HttpClient) {
    this.currentTokenSubject = new BehaviorSubject<Token>(
      JSON.parse(localStorage.getItem('currentToken'))
    );
    this.currentToken = this.currentTokenSubject.asObservable();
  }

  public get currentTokenValue(): Token {
    return this.currentTokenSubject.value;
  }

  login(log: Login) {
    return this.http.post<any>(`${config.apiUrl}/user/login`, log).pipe(
      map(res => {
        if (res && res.token) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem('currentToken', JSON.stringify(res));
          this.currentTokenSubject.next(res);
        }

        return res;
      })
    );
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentToken');
    this.currentTokenSubject.next(null);
  }

  register(reg: Register) {
    return this.http
      .post<any>(`${config.apiUrl}/user/register`, reg)
      .pipe(map(res => res));
  }

  getUsers(): Observable<User[]> {
    return this.http
      .get(`${config.apiUrl}/user/GetUsers`)
      .pipe(map((res: User[]) => res.map(data => new User(data))));
  }

  editUser(id: string, editUser: EditUser) {
    return this.http
      .put<any>(`${config.apiUrl}/user/edit/${id}`, editUser)
      .pipe(map(res => res));
  }

  active(id: string) {
    return this.http
      .get<any>(`${config.apiUrl}/user/active/${id}`)
      .pipe(map(res => res));
  }
}
