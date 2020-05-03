
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-oauth-callback',
  templateUrl: './oauth-callback.component.html',
})

export class OAuthCallbackComponent {
  constructor(private authService: AuthService, private router: Router) {

  }
  ngOnInit() {

  }
}
