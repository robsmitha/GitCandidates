import React, { Component } from 'react';
import { authService } from './../../services/auth.service'
import { Redirect, Link } from 'react-router-dom';
import { AuthConsumer } from './../../context/AuthContext';

export class Account extends Component {

    static displayName = Account.name;
    constructor(props) {
        super(props)
        this.state = {
            loading: true
        }
    }

    render() {
        return (
            <div>
                
                 <AuthConsumer>
                    {({ auth }) => (
                        <div>
                            {!auth
                                ? <Redirect to="/" />
                                :this.renderAccount()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        );
    }
    renderAccount() {
        return (
            <div>
                <p>My Account</p>
            </div>
            )
    }
}
