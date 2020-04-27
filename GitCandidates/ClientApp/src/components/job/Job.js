import React, { Component } from 'react';
import { Redirect, Link } from 'react-router-dom';
import { jobService } from '../../services/job.service';
import { userService } from '../../services/user.service';
import { AuthConsumer } from './../../context/AuthContext';
import Octicon, { Briefcase, Pencil, SignIn, Star, HeartOutline, Heart, PrimitiveDot, PrimitiveDotStroke } from '@primer/octicons-react';
import Loading from '../../helpers/Loading';

export class Job extends Component {

    constructor(props) {
        super(props)
        this.state = {
            id: this.props.match.params.id,
            job: null,
            skills: null
        }
    }

    componentDidMount() {
        this.populateJob()
        this.populateJobSkills()
    }

    populateJob() {
        jobService.getJob(this.state.id)
            .then(data => this.setState({
                job: data
            }))
    }

    populateJobSkills() {
        jobService.getJobSkills(this.state.id)
            .then(data => this.setState({
                skills: data
            }))
    }

    getButton(target) {
        return target instanceof HTMLButtonElement && target.getAttribute('type') == 'button'
            ? target
            : this.getButton(target.parentElement)
    }

    setSavedJob = event => {
        let btn = this.getButton(event.target)
        let id = Number(btn.value);
        let data = {
            savedJobID: id,
            jobID: Number(this.state.id)
        }
        userService.setSavedJob(data)
            .then(data => data ? this.populateJob() : console.log(data))
    }

    render() {
        return (
            <div>
                {this.renderLayout()}
            </div>
        );
    }

    renderLayout() {
        const { job, skills } = this.state
        return (
            <div>
                <div className="py-3">
                    <div className="container">
                        <div className="row">
                            <div className="col-md">
                                {job === null
                                    ? <Loading />
                                    : Job.renderJob(job, this.setSavedJob)}
                            </div>
                            <div className="col-md-4">
                                <div className="card mb-3">
                                    <h5 className="card-header bg-dark text-white">Job Skills</h5>
                                    <div className="card-body">
                                        {skills === null
                                            ? <Loading message="Loading skills" />
                                            : Job.renderSkills(skills)}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    static renderJob(job, setSavedJob) {
        return (
            <div>
                <div className="text-center">
                    <h1 className="h3">
                        {job.name}
                    </h1>
                    <strong className="d-block">
                        {job.companyGitHubLogin}
                    </strong>
                    <small className="d-block text-muted">
                        {job.locations.map((l, index) =>
                            <span key={l.id}>
                                <span>{l.location}</span>
                                {index < job.locations.length - 1
                                    ? <span>,</span>
                                    : <span></span>}
                            </span>
                        )}
                    </small>
                    <hr className="w-25" />
                </div>
                <div className="text-center mb-3">
                    {job.isAcceptingApplications
                        ? Job.renderApplyMessage(job, setSavedJob)
                        : <h5 className="text-muted">This job is no longer accepting applications.</h5>}
                </div>
                <p>
                    {job.description}
                </p>
            </div>
            )
    }
    static renderSkills(skills) {
        return (
            <div>
                <ul className="list-unstyled">
                    {skills.map(s =>
                        <li key={s.id}>
                            <Octicon icon={s.userHasSkill ? PrimitiveDot : PrimitiveDotStroke} size="small" />&nbsp;
                        {s.skillName}
                        </li>
                    )}
                </ul>
                <AuthConsumer>
                    {({ auth }) => (
                        <div>
                            {auth
                                ? <small className="text-muted">You have {skills.filter(s => s.userHasSkill).length} out of {skills.length} skills.</small>
                                : <span></span>}
                        </div>
                    )}
                </AuthConsumer>

            </div>
            )
    }
    static renderApplyMessage(job, setSavedJob) {
        return (
            <div>
                <AuthConsumer>
                    {({ auth }) => (
                        <div>
                            {!auth
                                ? <Link className="btn btn-link text-decoration-none" to="/oauth"><Octicon icon={SignIn} size="small" /> Sign in to apply.</Link>
                                : Job.renderAuthArea(job, setSavedJob)}
                        </div>
                    )}
                </AuthConsumer>
            </div>
            )
    }

    static renderAuthArea(job, setSavedJob) {
        return (
            <div>
                <div className="btn-group">
                    <Link className="btn btn-dark" to={'/apply/:id'.replace(':id', job.id)} hidden={!job.userCanApply}>
                        <Octicon icon={Pencil} size="small" /> Apply
                    </Link>
                    <button type="button" className="btn btn-outline-dark" value={job.savedJobID} onClick={setSavedJob}>
                        <Octicon icon={Star} size="small" /> {job.savedJobID > 0 ? 'Unsave' : 'Save'}
                    </button>
                </div>
                <div className="mt-2" hidden={job.userCanApply}>
                    <h5 className="text-muted">You have an active application.</h5>
                    <small className="text-muted">
                        Applications are displayed on your&nbsp;<Link to={'/account'}>account.</Link>
                    </small>
                </div>
            </div>
        )
    }

}