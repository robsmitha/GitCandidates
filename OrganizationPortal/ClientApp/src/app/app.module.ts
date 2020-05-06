import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { DatePipe } from '@angular/common';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { SignOutComponent } from './sign-out/sign-out.component';
import { OAuthComponent } from './oauth/oauth.component';
import { AccountComponent } from './account/account.component';
import { OAuthCallbackComponent } from './oauth-callback/oauth-callback.component';
import { OrganizationsComponent } from './organizations/organizations.component';
import { OrganizationComponent } from './organization/organization.component';

import { UserService } from './services/user.service';
import { AuthService } from './services/auth.service';
import { AuthGuard } from './utilities/auth.guard';
import { SiteLayoutComponent } from './_layout/site-layout/site-layout.component';
import { SiteHeaderComponent } from './_layout/site-header/site-header.component';
import { SiteFooterComponent } from './_layout/site-footer/site-footer.component';


import { FontAwesomeModule } from '@fortawesome/angular-fontawesome'

@NgModule({
  declarations: [
    AppComponent,
    SiteLayoutComponent,
    SiteHeaderComponent,
    SiteFooterComponent,
    HomeComponent,
    OAuthComponent,
    OAuthCallbackComponent,
    SignOutComponent,
    AccountComponent,
    OrganizationsComponent,
    OrganizationComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    RouterModule.forRoot([
      {
        path: '',
        component: SiteLayoutComponent,
        children: [
          { path: '', component: HomeComponent, pathMatch: 'full' },
          { path: 'account', component: AccountComponent, canActivate: [AuthGuard] },
          { path: 'organizations', component: OrganizationsComponent, canActivate: [AuthGuard] },
          { path: 'organization/:org', component: OrganizationComponent, canActivate: [AuthGuard], pathMatch: 'full' },
        ]
      },

      { path: 'oauth-callback', component: OAuthCallbackComponent },
      { path: 'oauth', component: OAuthComponent },
      { path: 'sign-out', component: SignOutComponent }
    ])
  ],
  providers: [
    UserService,
    AuthService,
    AuthGuard,
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
