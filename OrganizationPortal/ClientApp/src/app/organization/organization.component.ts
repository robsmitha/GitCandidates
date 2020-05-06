
import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
})

export class OrganizationComponent {
  gitHubOrganization = {}
  constructor(private userService: UserService,
    private router: Router,
    private route: ActivatedRoute) {

  }
  ngOnInit() {
    let org = this.route.snapshot.paramMap.get('org')
    this.userService.getOrganization(org)
      .subscribe(data => {
        console.log(data)
        this.gitHubOrganization = data.gitHubOrganization
      })
  }
}
