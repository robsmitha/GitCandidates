
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';
import { User } from '../models/user'
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-oauth',
  templateUrl: './oauth.component.html',
})

export class OAuthComponent {
  oAuthForm;
  user: User
  constructor(private authService: AuthService, private router: Router, private userService: UserService) {
    this.user = new User();
  }


  ngOnInit() {
    this.oAuthForm = new FormGroup({
      gitHubLogin: new FormControl(this.user.gitHubLogin, [
        Validators.required
      ])
    });
  }

  get gitHubLogin() {
    return this.oAuthForm.get('gitHubLogin')
  }

  onSubmit() {
    console.log(this.gitHubLogin.value)
    this.router.navigateByUrl('/account');
    /*
    this.authService.gitHubOAuthUrl(this.gitHubLogin)
      .subscribe(data => {
          window.location.href = data.url
        })
    */
  }
}
