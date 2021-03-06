
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { faAddressCard } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
})

export class AccountComponent {
  faAddressCard = faAddressCard;
  constructor(private userService: UserService, private router: Router) {

  }
  ngOnInit() {
    this.userService.getUser()
      .subscribe(data => {
        console.log(data)
      })
  }
}
