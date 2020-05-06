
import { Component } from '@angular/core';

@Component({
  selector: 'site-layout',
  templateUrl: './site-layout.component.html',
})

export class SiteLayoutComponent {
  title = "Site layout"
  constructor() {
    console.log('SiteLayoutComponent initialized')
  }
}
