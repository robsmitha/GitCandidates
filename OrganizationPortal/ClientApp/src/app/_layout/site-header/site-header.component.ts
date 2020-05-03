
import { Component } from '@angular/core';
import { AuthService } from './../../services/auth.service';

@Component({
  selector: 'site-header',
  templateUrl: './site-header.component.html'
})

export class SiteHeaderComponent {
  isExpanded = false;
  isDropdownExpanded = false;
  isLoggedIn = false
  constructor(private authService: AuthService) {

  }

  ngOnInit() {
    var navbar = document.querySelector('#main_nav').clientHeight;
    document.querySelector('#site_header').setAttribute('style', 'padding-top: ' + navbar + 'px')
  }

  collapse() {
    this.isExpanded = false;
    this.isDropdownExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
  toggleDropdown() {
    this.isDropdownExpanded = !this.isDropdownExpanded;
  }
}
