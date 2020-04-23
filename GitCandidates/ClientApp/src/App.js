import React, { Component } from 'react';
import LayoutRoute from './components/Layout';
import NoNavLayoutRoute from './components/NoNavLayout';
import { Home } from './components/Home';
import { OAuthCallback } from './components/OAuthCallback';
import { OAuth } from './components/OAuth';
import { SignOut } from './components/SignOut';
import { Account } from './components/account/Account';
import { Job } from './components/job/Job';
import { AuthProvider } from './context/AuthContext'

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
      return (
          <AuthProvider>
              <LayoutRoute exact path='/' component={Home} />
              <LayoutRoute exact path='/account' component={Account} />
              <LayoutRoute exact path='/job/:id' component={Job} />

              <NoNavLayoutRoute exact path='/oauth' component={OAuth} />
              <NoNavLayoutRoute exact path='/oauth-callback' component={OAuthCallback} />
              <NoNavLayoutRoute exact path='/sign-out' component={SignOut} />
          </AuthProvider>
    );
  }
}
