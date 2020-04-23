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
        userService.getUser().then(data => this.setState({ user: data, loading: false }))
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
            ? Account.renderLoading()
            : Account.renderUser(this.state.user)
        return (
            <div>
                {contents}
            </div>
        )
    }
    static renderUser(user) {
        return (

            <div className="d-flex align-items-stretch py-5">
                <div className="container">
                    <div className="row">
                        <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                            <div className="text-center">
                                <p className="lead">
                                    {user.name}
                                </p>
                                <a href={'https://github.com/:login'.replace(':login', user.login)} className="d-block text-decoration-none" target="_blank">
                                    {user.login}
                                </a>
                                <small className="d-block text-muted">
                                    {user.location}
                                </small>
                                <hr className="w-25" />
                                <p>
                                    {user.bio}
                                </p>
                            </div>

                            <div className="card-body pt-2">
                                <div className="mx-auto">
                                    {user.repos
                                        .filter(repo => repo.description !== null && repo.description.length > 0)
                                        .map(repo =>
                                            <div className="media text-muted" key={repo.id}>
                                                <span className="mr-2 mt-2 rounded h1">

                                                </span>
                                                <div className="media-body mt-2 pb-3 mb-0 small lh-125 border-bottom border-gray">
                                                    <div className="d-flex justify-content-between align-items-center w-100">
                                                        <h6 className="text-primary mb-1">{repo.name}</h6>
                                                    </div>
                                                    <span className="d-block">{repo.description}</span>
                                                </div>
                                            </div>
                                        )}
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        )
    }

    static renderLoading() {
        return (
            <div className="d-flex align-items-stretch py-5">
                <div className="container">
                    <div className="row">
                        <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                            <div className="text-center">
                                <p className="lead">
                                    <span className="spinner-grow" role="status">
                                        <span className="sr-only">Loading...</span>
                                    </span>
                                    Loading account..
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
