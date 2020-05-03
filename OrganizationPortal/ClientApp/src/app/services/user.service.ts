
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../models/user';

@Injectable()
export class UserService {

  constructor(private client: HttpClient) {}


}
