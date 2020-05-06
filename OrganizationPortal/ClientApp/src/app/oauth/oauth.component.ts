
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';
import { User } from '../models/user'
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { faSignInAlt, faBullhorn, faSearch, faCodeBranch } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-oauth',
  templateUrl: './oauth.component.html',
})

export class OAuthComponent {
  faSignInAlt = faSignInAlt;
  faBullhorn = faBullhorn;
  faCodeBranch = faCodeBranch;
  faSearch = faSearch;
  oAuthForm;
  user: User
  constructor(private authService: AuthService, private router: Router, private userService: UserService) {
    this.user = new User();
    if (authService.appUser !== null && authService.appUser.auth) {
      this.router.navigateByUrl('/account');
    }
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
    this.authService.gitHubOAuthUrl(this.gitHubLogin.value)
      .subscribe(data => {
          window.location.href = data.url
        })
  }
}
