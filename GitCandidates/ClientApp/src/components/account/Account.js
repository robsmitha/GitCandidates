import React, { Component } from 'react';
import { userService } from '../../services/user.service'
import { Redirect, Link } from 'react-router-dom';
import { AuthConsumer } from '../../context/AuthContext';
import { NavAccountMenu } from './NavAccountMenu';
import Loading from '../../helpers/Loading';
import GitHubUser from '../../helpers/GitHubUser';
import Octicon, { Trashcan } from '@primer/octicons-react';

export class Account extends Component {

    static displayName = Account.name;
    constructor(props) {
        super(props)
        this.state = {
            loading: true,
            user: null,
            applications: null,
            inactiveApplications: null
        }
    }

    componentDidMount() {
        this.populateUser()
        this.populateApplications()
    }

    populateUser = () => {
        userService.getUser()
            .then(data => this.setState({
                user: data
            }))
    }

    populateApplications = () => {
        userService.getJobApplications()
            .then(data => this.setState({
                applications: data.jobApplications,
                inactiveApplications: data.inactiveJobApplications
            }))
    }

    getButton(target) {
        return target instanceof HTMLButtonElement && target.getAttribute('type') == 'button'
            ? target
            : this.getButton(target.parentElement)
    }

    confirmWithdrawlApplication = event => {
        let btn = this.getButton(event.target)
        let name = btn.getAttribute('data-name')
        let company = btn.getAttribute('data-company')
        let id = Number(btn.value)
        if (window.confirm(`Are you sure you want to withdaw application #${id} for ${name} at ${company}?`)) {
            userService.withdrawApplication({
                id: id
            }).then(data => {
                if (data) {
                    this.populateApplications()
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
        const { applications, inactiveApplications, user } = this.state
        return (
            <div>
                <div className="py-3">
                    <div className="container">
                        <div className="row">
                            <div className="col-lg-2 col-md-3 col-sm-4 col-12">
                                <div className="text-center">
                                    {user === undefined || user === null
                                        ? <Loading />
                                        : <GitHubUser user={user.gitHubUser} />}
                                </div>
                            </div>
                            <div className="col">
                                <NavAccountMenu />
                                <div>
                                    <h4 className="pl-3">Applications</h4>
                                    {applications !== undefined && inactiveApplications !== undefined
                                        && applications !== null && inactiveApplications !== null
                                        ? Account.renderApplications(applications, inactiveApplications, this.confirmWithdrawlApplication)
                                        : <Loading message="Loading saved jobs" />}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
    static renderApplications(applications, inactiveApplications, confirmWithdrawlApplication) {
        return (
            <div>
                <h6 className="pl-3 text-muted">Active applications</h6>
                <div className="list-group list-group-flush mb-3">
                    {applications.map((a, index) =>
                        <div className="list-group-item" key={a.id}>
                            <div className="d-flex w-100 justify-content-between">
                                <Link to={'/job/:id'.replace(':id', a.jobID)} className="text-decoration-none h5 mb-1">
                                    {a.jobName}
                                </Link>
                                <small className="text-right">Submitted on {a.updatedOn}</small>
                            </div>
                            <p className="mb-0">
                                {a.jobCompanyGitHubLogin}
                                <button className="btn btn-link btn-sm text-muted float-right pr-0" type="button" value={a.id} data-name={a.jobName} data-company={a.jobCompanyGitHubLogin} onClick={confirmWithdrawlApplication}>
                                    <Octicon icon={Trashcan} size="small" />
                                    <span className="sr-only">
                                        Withdraw
                                    </span>
                                </button>
                            </p>
                            <small className="text-muted">
                                <strong>Status:&nbsp;</strong>
                                {a.jobApplicationStatusTypeName}
                            </small>
                        </div>
                    )}
                    <div className="list-group-item" hidden={applications.length > 0}>
                        <p className="lead">
                            You have no active applications to display. <Link to={'/'}>Click here</Link> to see open opportunities.
                                    </p>
                    </div>
                </div>

                <h6 className="pl-3 text-muted">Archived applications</h6>
                <div className="list-group list-group-flush">
                    {inactiveApplications.map((a, index) =>
                        <div className="list-group-item list-group-item-light" key={a.id}>
                            <div className="d-flex w-100 justify-content-between">
                                <Link to={'/job/:id'.replace(':id', a.jobID)} className="text-decoration-none h5 mb-1">
                                    {a.jobName}
                                </Link>
                                <small className="text-right">Updated on {a.updatedOn}</small>
                            </div>
                            <p className="mb-1">{a.jobCompanyGitHubLogin}</p>
                            <small className="d-text-muted">{a.jobApplicationStatusTypeName}</small>
                        </div>
                    )}
                    <div className="list-group-item" hidden={inactiveApplications.length > 0}>
                        <p className="lead">
                            You have no inactive applications to display.
                                    </p>
                    </div>
                </div>
            </div>
            )
    }
}
