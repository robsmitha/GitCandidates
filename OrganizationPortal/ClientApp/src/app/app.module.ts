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
import { PageHeaderComponent } from './_layout/page-header/page-header.component';
import { OAuthCallbackComponent } from './oauth-callback/oauth-callback.component';

import { UserService } from './services/user.service';
import { AuthService } from './services/auth.service';
import { AuthGuard } from './utilities/auth.guard';
import { SiteLayoutComponent } from './_layout/site-layout/site-layout.component';
import { SiteHeaderComponent } from './_layout/site-header/site-header.component';
import { SiteFooterComponent } from './_layout/site-footer/site-footer.component';




@NgModule({
  declarations: [
    AppComponent,
    SiteLayoutComponent,
    SiteHeaderComponent,
    SiteFooterComponent,
    PageHeaderComponent,
    HomeComponent,
    OAuthComponent,
    OAuthCallbackComponent,
    SignOutComponent,
    AccountComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      {
        path: '',
        component: SiteLayoutComponent,
        children: [
          { path: '', component: HomeComponent, pathMatch: 'full' },
          { path: 'account', component: AccountComponent, canActivate: [AuthGuard] },
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
