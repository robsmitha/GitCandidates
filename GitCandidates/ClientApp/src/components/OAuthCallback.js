import React, { Component } from 'react';
import { authService } from './../services/auth.service'
import { Redirect, Link } from 'react-router-dom';
import { AuthConsumer, AuthContext } from './../context/AuthContext';
import Octicon, { Lock, Telescope } from '@primer/octicons-react';

export class OAuthCallback extends Component {

    static displayName = OAuthCallback.name;
    constructor(props) {
        super(props)
        this.state = {
            loading: true
        }
    }

    componentDidMount() {
        let search = window.location.search;
        let params = new URLSearchParams(search);
        let code = params.get('code');
        let state = params.get('state');
        if (code !== null && code.length > 0 && state !== null && state.length > 0) {
            authService.gitHubOAuthCallback({ code: code, state: state })
                .then(data => {
                    if (data) {
                        this.context.signIn();
                    }
                    else {
                        //an error occurred
                        this.setState({
                            loading: false
                        })
                    }
                })

            //the code will expire after 10 minutes. 
            setTimeout(() => {
                window.location.href = '/'
            }, 600000)
        }
        else {
            //code & state not present
            window.location.href = '/'
        }
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ auth }) => (
                        <div>
                            {auth
                                ? <Redirect to="/account" />
                                : this.renderLoading()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        );
    }


    renderLoading() {
        return (
            <div>
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
                                <div className="text-center">
                                    <p className="lead" hidden={!this.state.loading}>
                                        <span className="spinner-grow" role="status">
                                            <span className="sr-only">Loading...</span>
                                        </span>
                                        Authorizing your account..
                                    </p>
                                    <div hidden={this.state.loading}>
                                        Could not complete set up. Please&nbsp;<Link to='/'>try again.</Link>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            )
    }
}
OAuthCallback.contextType = AuthContext; 