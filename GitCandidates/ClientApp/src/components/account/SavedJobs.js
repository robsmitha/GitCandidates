import React, { Component } from 'react';
import { Redirect, Link } from 'react-router-dom';
import { AuthConsumer } from './../../context/AuthContext';
import { NavAccountMenu } from './NavAccountMenu';
import { userService } from '../../services/user.service'
import GitHubUser from '../../helpers/GitHubUser';
import Loading from '../../helpers/Loading';
import Octicon, { Star } from '@primer/octicons-react';

export class SavedJobs extends Component {
    constructor(props) {
        super(props)
        this.state = {
            savedJobs: null,
            user: null
        }
    }
    componentDidMount() {
        this.populateUser()
        this.populateSavedJobs()
    }

    populateUser = () => {
        userService.getUser()
            .then(data => this.setState({ user: data }))
    }

    populateSavedJobs = () => {
        userService.getSavedJobs()
            .then(data => this.setState({ savedJobs: data }))
    }

    getButton(target) {
        return target instanceof HTMLButtonElement && target.getAttribute('type') == 'button'
            ? target
            : this.getButton(target.parentElement)
    }
    setSavedJob = event => {
        let btn = this.getButton(event.target)
        let jobId = btn.getAttribute('data-job-id')
        let name = btn.getAttribute('data-name')
        let company = btn.getAttribute('data-company')
        if (window.confirm(`Are you sure you want to unsave the job: ${name} at ${company}?`)) {
            let data = {
                jobID: Number(jobId)
            }
            userService.setSavedJob(data)
                .then(data => data ? this.populateSavedJobs() : console.log(data))
        }
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ auth }) => (
                        <div>
                            {!auth
                                ? <Redirect to="/oauth" />
                                : this.renderLayout()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        );
    }

    renderLayout() {
        const { savedJobs, user } = this.state
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
                                    <h4 className="pl-3">Saved Jobs</h4>
                                    {savedJobs === null
                                        ? <Loading message="Loading saved jobs" />
                                        : SavedJobs.renderSavedJobs(savedJobs, this.setSavedJob)}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    static renderSavedJobs(savedJobs, setSavedJob) {
        return (
            <div>
                <div className="list-group list-group-flush mb-3">
                    {savedJobs.map((a, index) =>
                        <div className="list-group-item" key={a.jobID}>
                            <div className="d-flex w-100 justify-content-between">
                                <Link to={'/job/:id'.replace(':id', a.jobID)} className="text-decoration-none h5 mb-1">
                                    {a.jobName}
                                </Link>
                                <small className="text-right">Posted {a.posted}</small>
                            </div>
                            <p className="mb-0">
                                {a.jobCompanyGitHubLogin}
                                <button className="btn btn-link btn-sm float-right pr-0"
                                    type="button"
                                    data-job-id={a.jobID}
                                    data-name={a.jobName}
                                    data-company={a.jobCompanyGitHubLogin}
                                    onClick={setSavedJob}>
                                    <Octicon icon={Star} size="small" /> Unsave
                                </button>
                            </p>
                        </div>
                    )}
                    <div className="list-group-item" hidden={savedJobs.length > 0}>
                        <p className="lead">
                            You have no active saved jobs to display. <Link to={'/'}>Click here</Link> to see open opportunities.
                        </p>
                    </div>
                </div>
            </div>
            )
    }
}