
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-sign-out',
  templateUrl: './sign-out.component.html',
})

export class SignOutComponent {
  constructor(private authService: AuthService, private router: Router) {
    
  }
  ngOnInit() {

  }
}
