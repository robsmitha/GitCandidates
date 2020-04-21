import React, { Component } from 'react';
import { authService } from './../services/auth.service'
import TextInput from './../helpers/TextInput';
import handleChange from './../helpers/HandleChange';
import { Redirect } from 'react-router-dom';
import { AuthConsumer } from './../context/AuthContext';


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

            <div className="vh-100 d-flex align-items-stretch py-5">
                <div className="container">
                    <div className="row">
                        <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                            <h1 className="h3 mb-4 text-center">
                                GitCandidates
                            </h1>
                            <h2 className="h4 mb-2">Sign in with your GitHub account</h2>
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

                                <p className="text-center mt-3 small text-muted">
                                    Don't have an account? Just create one!
                                </p>
                                <button className="btn btn-link btn-block text-decoration-none" type="submit" onClick={this.submitHandler}>Continue to GitHub</button>
                            </form>
                           
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
