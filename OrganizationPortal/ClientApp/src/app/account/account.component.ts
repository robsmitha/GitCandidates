
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
})

export class AccountComponent {
  constructor(private authService: AuthService, private router: Router) {

  }
  ngOnInit() {

  }
}
