
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-organizations',
  templateUrl: './organizations.component.html',
})

export class OrganizationsComponent {
  organizations = []
  constructor(private userService: UserService, private router: Router) {

  }
  ngOnInit() {
    this.userService.getOrganizations()
      .subscribe(data => {
        this.organizations = data.organizations
      })
  }
}
