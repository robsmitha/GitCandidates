import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { AuthConsumer } from './../context/AuthContext';


export class Home extends Component {

    static displayName = Home.name;
    constructor(props) {
        super(props)
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ auth }) => (
                        <div>
                            {auth
                                ? <p>Hello, Candidate</p>
                                : this.renderSignInMessage()}

                        </div>
                    )}
                </AuthConsumer>
            </div>
        );
    }
    renderSignInMessage() {
        return (
            <div>
                <p>Welcome</p>
                <small>Please <Link to="/oauth">sign in</Link></small>
            </div>
            )
    }
}
