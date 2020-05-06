
import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-oauth-callback',
  templateUrl: './oauth-callback.component.html',
})

export class OAuthCallbackComponent {
  constructor(private authService: AuthService, private router: Router,
    private route: ActivatedRoute) {

  }
  ngOnInit() {
    let search = window.location.search;
    let params = new URLSearchParams(search);
    let code = params.get('code');
    let state = params.get('state');
    if (code !== null && code.length > 0 && state !== null && state.length > 0) {
      this.authService.gitHubOAuthCallback({ code: code, state: state })
        .subscribe(data => {
          if (data) {
            this.authService.setAppUser(data)
            this.router.navigateByUrl('/account');
          }
          else {
            alert('An error occurred. Please try again.')
            this.router.navigateByUrl('/oauth')
          }
        })
    }
  }
}
