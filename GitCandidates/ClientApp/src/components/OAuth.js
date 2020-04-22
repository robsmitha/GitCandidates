import React, { Component } from 'react';
import { authService } from './../services/auth.service'
import TextInput from './../helpers/TextInput';
import handleChange from './../helpers/HandleChange';
import { Redirect, Link } from 'react-router-dom';
import { AuthConsumer } from './../context/AuthContext';
import Octicon, { Code, Search, Lock, Telescope } from '@primer/octicons-react'


export class OAuth extends Component {
    static displayName = OAuth.name;
    constructor(props) {
        super(props)
        this.state = {
            formIsValid: false,
            formControls: {
                login: {
                    value: '',
                    placeholder: 'GitHub login',
                    label: 'GitHub login',
                    valid: false,
                    touched: false,
                    validationRules: {
                        isRequired: true,
                        minLength: 1
                    },
                    errors: []
                },
                staySignedIn: {
                    value: true,
                    label: 'Stay signed in',
                    valid: true,
                    touched: false
                }
            }
        }
    }

    changeHandler = event => {
        const name = event.target.name;
        const value = event.target instanceof HTMLInputElement && event.target.getAttribute('type') == 'checkbox'
            ? event.target.checked
            : event.target.value;
        this.setState(handleChange(name, value, this.state.formControls));
    }

    submitHandler = event => {
        event.preventDefault()
        const login = this.state.formControls.login.value
        authService.gitHubOAuthUrl(login)
            .then(data => {
                window.location.href = data.url
            })
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ auth }) => (
                        <div>
                            {auth
                                ? <Redirect to="/account" />
                                : this.renderOAuthForm()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        );
    }

    renderOAuthForm() {
        return (
            <div>
                <div className="container-fluid">
                    <div className="row no-gutter">
                        <div className="d-none d-md-block col-md-4 col-lg-6 bg-light border-right">
                            <div className="vh-100 d-flex align-items-stretch py-5">
                                <div className="container">
                                    <div className="row">
                                        <div className="col-md-9 col-lg-8 mx-auto">
                                            <Octicon icon={Telescope} size="medium" />&nbsp;
                                            <span className="h3">GitCandidates</span>
                                            <p>
                                                Finding the perfect job octo be easy.
                                            </p>
                                            <ol className="fa-ul list-unstyled">
                                                <li className="mb-3">
                                                    <Octicon icon={Lock} />
                                                    <strong className="mb-2 ml-2">Secure GitHub access</strong>
                                                    <small className="d-block ml-4">Enter your GitHub username to securely authenticate your GitHub account.</small>
                                                </li>
                                                <li className="mb-3">
                                                    <Octicon icon={Search} />
                                                    <strong className="mb-2 ml-2">Flexible search tools</strong>
                                                    <small className="d-block ml-4">Browse job opportunities by company, languages, frameworks and more!</small>
                                                </li>
                                                <li className="mb-3">
                                                    <Octicon icon={Code} />
                                                    <strong className="mb-2 ml-2">See what you can do</strong>
                                                    <small className="d-block ml-4">Each company puts out some repos related to the needs of the positions.</small>
                                                </li>
                                            </ol>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="col-md-8 col-lg-6">
                            <div className="vh-100 d-flex align-items-stretch py-5">
                                <div className="container">
                                    <div className="row">
                                        <div className="col-md-9 col-lg-8 mx-auto">
                                            <h1 className="h3">
                                                Sign in with GitHub
                                            </h1>
                                            <p>Are you a company? <Link to="/" className="text-decoration-none">Sign in here.</Link></p>

                                            <form method="post" onSubmit={this.submitHandler}>

                                                <TextInput name="login"
                                                    placeholder={this.state.formControls.login.placeholder}
                                                    label={this.state.formControls.login.label}
                                                    value={this.state.formControls.login.value}
                                                    onChange={this.changeHandler}
                                                    touched={this.state.formControls.login.touched ? 1 : 0}
                                                    valid={this.state.formControls.login.valid ? 1 : 0}
                                                    errors={this.state.formControls.login.errors} />


                                                <div className="custom-control custom-checkbox">
                                                    <input type="checkbox" className="custom-control-input" id="staySignedIn" name="staySignedIn" onChange={this.changeHandler} checked={this.state.formControls.staySignedIn.value} />
                                                    <label className="custom-control-label" htmlFor="staySignedIn">
                                                        Stay signed in
                                                    </label>
                                                </div>

                                                <button type="submit" disabled={!this.state.formIsValid} className="btn btn-dark btn-block my-3">
                                                    Sign in with GitHub
                                                </button>

                                                <p className="text-center text-muted">
                                                    Don't have an account?
                                                    <button className="btn btn-link btn-sm text-decoration-none pt-0 pl-1 small" type="submit" onClick={this.submitHandler}>Sign up.</button>
                                                </p>

                                            </form>
                                        </div>
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
