
import { Component } from '@angular/core';
import { AuthService } from './../../services/auth.service';
import { faUser, faPlus } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'site-header',
  templateUrl: './site-header.component.html',
  host: {
    '(document:click)': 'onOutsideClick($event)',
  }
})

export class SiteHeaderComponent {
  isExpanded: boolean = false;
  isDropdownExpanded: boolean = false;
  isPlusDropdownExpanded: boolean = false;
  faUser = faUser;
  faPlus = faPlus;
  gitHubLogin: string
  constructor(private authService: AuthService) {
    this.gitHubLogin = authService !== undefined && authService !== null && authService.appUser !== undefined && authService.appUser !== null
      ? authService.appUser.gitHubLogin
      : null;
  }

  onOutsideClick = event => {
    if (!event.target.classList.contains('dropdown-toggle')) {
      this.findDropdownToggle(event.target)
    }
  }

  findDropdownToggle(elem) {
    console.log(elem)
    if (elem === undefined || elem === null) {
      //did not find
      return
    }

    if (elem.classList.contains('dropdown')) {
      let dropdowns = document.querySelector('#main_nav').querySelectorAll('.dropdown')
      for (var i = 0; i < dropdowns.length; i++) {
        if (dropdowns[i] !== elem && dropdowns[i].classList.contains('show')) {
          console.log('close')
        }
      }
      return
    }

    if (elem.nodeName.toLowerCase() === 'path'
      || elem.nodeName.toLowerCase() === 'svg'
      || elem.nodeName.toLowerCase() === 'fa-icon'
      || elem.classList.contains('dropdown-toggle')) {
      this.findDropdownToggle(elem.parentElement)
    }
    else if (this.isExpanded || this.isDropdownExpanded || this.isPlusDropdownExpanded) {
        this.collapse()
    }
  }

  ngOnInit() {
    var navbar = document.querySelector('#main_nav').clientHeight;
    document.querySelector('#site_header').setAttribute('style', 'padding-top: ' + navbar + 'px')
  }

  collapse() {
    this.isExpanded = false;
    this.isDropdownExpanded = false;
    this.isPlusDropdownExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
  toggleDropdown() {
    this.isPlusDropdownExpanded = false;
    this.isDropdownExpanded = !this.isDropdownExpanded;
  }
  togglePlusDropdown() {
    this.isDropdownExpanded = false;
    this.isPlusDropdownExpanded = !this.isPlusDropdownExpanded;
  }

  get isLoggedIn(): boolean {
    const { appUser } = this.authService;
    return appUser === undefined || appUser === null
      ? false
      : appUser.auth;
  }
}
