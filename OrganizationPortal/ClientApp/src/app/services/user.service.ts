
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../models/user';

@Injectable()
export class UserService {

  constructor(private client: HttpClient) {}

  getUser(): Observable<any> {
    return this.client.get('users/getuser')
  }

  getOrganizations(): Observable<any> {
    return this.client.get('users/getOrganizations')
  }

  getOrganization(organization: string): Observable<any> {
    return this.client.get(`users/getOrganization/${organization}`)
  }
}
