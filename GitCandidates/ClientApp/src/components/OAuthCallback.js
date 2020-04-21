import React, { Component } from 'react';
import { authService } from './../services/auth.service'
import { Redirect, Link } from 'react-router-dom';
import { AuthConsumer, AuthContext } from './../context/AuthContext';

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
                <p className="lead" hidden={!this.state.loading}>
                    <span className="spinner-grow" role="status">
                        <span className="sr-only">Loading...</span>
                    </span>
                    Setting up your account..
                </p>
                <div hidden={this.state.loading}>
                    Could not complete set up. Please&nbsp;<Link to='/'>try again.</Link>
                </div>
            </div>
            )
    }
}
OAuthCallback.contextType = AuthContext; 