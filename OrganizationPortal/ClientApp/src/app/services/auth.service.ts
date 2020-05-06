import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable()
export class AuthService {

  constructor(private client: HttpClient) {}


  gitHubOAuthUrl(login: string): Observable<any> {
    return this.client.get(`auth/GitHubOAuthUrl/${login}`)
  }

  gitHubOAuthCallback(data): Observable<any> {
    return this.client.post('auth/GitHubOAuthCallback', data);
  }

  authorize(data): Observable<any> {
    return this.client.post('auth/authorize', data);
  }

  signOut(): Observable<any> {
    return this.client.post('auth/signOut', null);
  }

  _appUserKey: string = 'appUser';

  setAppUser(data) {
    localStorage.setItem(this._appUserKey, JSON.stringify(data))
  }

  clearAppUser() {
    localStorage.removeItem(this._appUserKey);
  }

  get appUser() {
    var item = localStorage.getItem(this._appUserKey)

    if (item != null && item.length > 0)
      return JSON.parse(item)

    return {
      auth: false,
      gitHubLogin: null
    }
  }
}
