import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;
    constructor(props) {
        super(props)
        this.state = {
            oAuthUrl: 'https://github.com/login/oauth/authorize'
        }
    }


    render() {
        return (
            <div>
                <h1>Hello, candidate!</h1>
                <p>Sign in with your GitHub profile to get started: </p>
                <a href={this.state.oAuthUrl}>
                    Sign in with GitHub
                </a>
            </div>
        );
    }
}
