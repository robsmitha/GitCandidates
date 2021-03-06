import React, { Component } from 'react';
import LayoutRoute from './components/Layout';
import NoNavLayoutRoute from './components/NoNavLayout';
import { Home } from './components/Home';
import { OAuthCallback } from './components/OAuthCallback';
import { OAuth } from './components/OAuth';
import { SignOut } from './components/SignOut';
import { HowItWorks } from './components/HowItWorks';
import { Account } from './components/account/Account';
import { Settings } from './components/account/Settings';
import { SavedJobs } from './components/account/SavedJobs';
import { Collaborations } from './components/account/Collaborations';
import { Job } from './components/job/Job';
import { AuthProvider } from './context/AuthContext'

import './custom.css'
import { Apply } from './components/job/Apply';

export default class App extends Component {
  static displayName = App.name;

  render () {
      return (
          <AuthProvider>
              <LayoutRoute exact path='/' component={Home} />
              <LayoutRoute exact path='/how-it-works' component={HowItWorks} />
              <LayoutRoute exact path='/account' component={Account} />
              <LayoutRoute exact path='/settings' component={Settings} />
              <LayoutRoute exact path='/saved-jobs' component={SavedJobs} />
              <LayoutRoute exact path='/collaborations' component={Collaborations} />
              <LayoutRoute exact path='/job/:id' component={Job} />
              <LayoutRoute exact path='/apply/:id' component={Apply} />

              <NoNavLayoutRoute exact path='/oauth' component={OAuth} />
              <NoNavLayoutRoute exact path='/oauth-callback' component={OAuthCallback} />
              <NoNavLayoutRoute exact path='/sign-out' component={SignOut} />
          </AuthProvider>
    );
  }
}
