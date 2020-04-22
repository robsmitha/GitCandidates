import React, { Component } from 'react';
import { userService } from './../../services/user.service'
import { Redirect, Link } from 'react-router-dom';
import { AuthConsumer } from './../../context/AuthContext';

export class Account extends Component {

    static displayName = Account.name;
    constructor(props) {
        super(props)
        this.state = {
            loading: true,
            user: null
        }
    }

    componentDidMount() {
        userService.getUser().then(data => this.setState({ user: data, loading: false}))
    }

    render() {
        return (
            <div>
                
                 <AuthConsumer>
                    {({ auth }) => (
                        <div>
                            {!auth
                                ? <Redirect to="/" />
                                : this.renderLayout()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        );
    }
    renderLayout() {
        let contents = this.state.loading
            ? <p>Loading..</p>
            : Account.renderUser(this.state.user)
        return (
            <div>
                {contents}
            </div>
            )
    }
    static renderUser(user) {
        return (
            <div>
                <div className="container">
                    <h2>{user.name}</h2>
                    <small className="d-block">{user.login}</small>
                    <p>{user.bio}</p>
                </div>
            </div>
            )
    }
}
