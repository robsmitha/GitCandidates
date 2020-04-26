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
        this.populateUserApplications()
    }

    populateUserApplications = () => {
        userService.getUser().then(data => this.setState({ user: data, loading: false }))
    }

    confirmWithdrawlApplication = event => {
        let name = event.target.getAttribute('data-name')
        let company = event.target.getAttribute('data-company')
        let id = Number(event.target.value)
        if (window.confirm(`Are you sure you want to withdaw application #${id} for ${name} at ${company}`)) {
            userService.withdrawApplication({
                id: id
            }).then(data => {
                if (data) {
                    this.populateUserApplications()
                }
                else {
                    console.log(data)
                }
            })
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
            : Account.renderUser(this.state.user, this.confirmWithdrawlApplication)
        return (
            <div>
                {contents}
            </div>
        )
    }
    static renderUser(user, confirmWithdrawlApplication) {
        const { gitHubUser } = user
        return (

            <div className="d-flex align-items-stretch py-5">
                <div className="container">
                    <div className="row">
                        <div className="col-lg-2 col-md-3 col-sm-4 col-12">
                            <div className="text-center">
                                <img className="img-fluid w-50" src={gitHubUser.avatar_url} loading="lazy" />
                                <strong className="d-block">{gitHubUser.name}</strong>
                                <a href={gitHubUser.html_url} className="d-block text-decoration-none" target="_blank">
                                    {gitHubUser.login}
                                </a>
                                <hr className="w-25 my-1" />
                            </div>
                        </div>
                        <div className="col">
                            <h4 className="pl-3">Active Applications</h4>
                            <div className="list-group list-group-flush mb-3">
                                {user.jobApplications.map((a, index) =>
                                    <div className="list-group-item" key={a.id}>
                                        <div className="d-flex w-100 justify-content-between">
                                            <Link to={'/job/:id'.replace(':id', a.jobID)} className="text-decoration-none h5 mb-1">
                                                {a.jobName}
                                            </Link>
                                            <small>{a.updatedOn}</small>
                                        </div>
                                        <p className="mb-0">
                                            {a.jobCompanyGitHubLogin}&middot;
                                            <button className="btn btn-link btn-sm text-danger float-right pr-0" type="button" value={a.id} data-name={a.jobName} data-company={a.jobCompanyGitHubLogin} onClick={confirmWithdrawlApplication}>Withdraw</button>
                                        </p>
                                        <small className="text-muted">
                                            <strong>Status:&nbsp;</strong>
                                            {a.jobApplicationStatusTypeName}
                                        </small>
                                    </div>
                                )}
                                <div className="list-group-item" hidden={user.jobApplications.length > 0}>
                                    <p className="lead">
                                        You have no active applications to display. <Link to={'/'}>Click here</Link> to see open opportunities.
                                    </p>
                                </div>
                            </div>

                            <h4 className="pl-3">Inactive Applications</h4>
                            <div className="list-group list-group-flush">
                                {user.inactiveJobApplications.map((a, index) =>
                                    <div className="list-group-item list-group-item-light" key={a.id}>
                                        <div className="d-flex w-100 justify-content-between">
                                            <Link to={'/job/:id'.replace(':id', a.jobID)} className="text-decoration-none h5 mb-1">
                                                {a.jobName}
                                            </Link>
                                            <small>{a.updatedOn}</small>
                                        </div>
                                        <p className="mb-1">{a.jobCompanyGitHubLogin}</p>
                                        <small className="d-text-muted">{a.jobApplicationStatusTypeName}</small>
                                    </div>
                                )}
                                <div className="list-group-item" hidden={user.inactiveJobApplications.length > 0}>
                                    <p className="lead">
                                        You have no inactive applications to display.
                                    </p>
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
