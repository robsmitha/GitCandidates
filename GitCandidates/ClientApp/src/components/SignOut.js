﻿import React, { Component } from 'react';
import { Link, Redirect } from 'react-router-dom'
import { AuthConsumer } from './../context/AuthContext'
import Octicon, { Telescope } from '@primer/octicons-react';

export class SignOut extends Component {
    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ auth, signOut }) => (
                        <div>
                            {!auth
                                ? <Redirect to='/' />
                                : this.renderSignOut(signOut)}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        )
    }
    renderSignOut(signOut) {
        return (
            <div className="vh-100 d-flex align-items-stretch py-5">
                <div className="container">
                    <div className="row">
                        <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                            <div className="mb-4 text-center">
                                <Octicon icon={Telescope} size="medium" />&nbsp;
                                <span className="h3">
                                        GitCandidates
                                </span>
                            </div>
                            <h2 className="h4 mb-2">Are you sure you want to sign out?</h2>
                            <button type="button" onClick={signOut} className="btn btn-dark btn-block my-3">
                                Yes, sign me out
                            </button>
                            <Link to='/account' className="btn btn-link btn-block text-decoration-none">
                                No, keep me signed in
                            </Link>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}