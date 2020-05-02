import React, { Component } from 'react';
import { Redirect, Link } from 'react-router-dom';
import { jobService } from '../../services/job.service';
import { userService } from '../../services/user.service';
import { AuthConsumer, AuthContext } from './../../context/AuthContext';
import Octicon, { SignIn, Star, Check, History, Pin, Pencil, Checklist, Jersey, Pulse, ChevronRight, Heart, HeartOutline, Code } from '@primer/octicons-react';
import Loading from '../../helpers/Loading';
import { Row, Col } from 'reactstrap';

export class Job extends Component {

    constructor(props) {
        super(props)
        this.state = {
            id: this.props.match.params.id,
            job: null,
            skills: null,
            loggedIn: false
        }
    }

    componentDidMount() {
        this.setState({
            loggedIn: this.context.auth
        })
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

        if (!this.state.loggedIn) this.props.history.push('/oauth')

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
        const { job, skills, loggedIn } = this.state
        return (
            <div>
                <div className="py-3">
                    <div className="container">
                        <div className="row">
                            <div className="col-md">
                                {job === null
                                    ? <Loading message="Loading opportunity, please wait." />
                                    : Job.renderJob(job, loggedIn, this.setSavedJob)}
                            </div>
                            <div className="col-md-3">
                                <div className="mb-3">
                                    <h4 className="mb-1 h6 text-primary text-uppercase">
                                        Need
                                    </h4>
                                    <div>
                                        <p className="h5 mb-3">
                                            Who we need
                                        </p>
                                    </div>
                                    {job == null
                                        ? <Loading />
                                        : <p className="lead mb-3">
                                            {job.description}
                                        </p>}
                                </div>
                                <div className="mb-3">
                                    <h4 className="mb-1 h6 text-primary text-uppercase">
                                        Stack
                                    </h4>
                                    <div>
                                        <p className="h5 mb-3">
                                            Our code
                                        </p>
                                    </div>
                                    {skills === null
                                        ? <Loading />
                                        : Job.renderSkills(skills)}
                                </div>
                                <div className="mb-3">
                                    <h4 className="mb-1 h6 text-primary text-uppercase">
                                        Approach
                                    </h4>
                                    <div>
                                        <p className="h5 mb-3">
                                            Our methods
                                        </p>
                                    </div>
                                    {job === null
                                        ? <Loading />
                                        : Job.renderMethods(job.methods)}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    static renderJob(job, loggedIn, setSavedJob) {
        return (
            <div>
                <div className="mb-3">
                    <h1 className="mb-1 h6 text-primary text-uppercase">
                        {job.companyGitHubLogin}
                    </h1>
                    <Row>
                        <Col>
                            <h2 className="h3 mb-0">
                                {job.name}
                            </h2>
                        </Col>
                        <Col xs="auto">
                            <button type="button" className={'btn btn-sm rounded-circle text-danger py-0'} value={job.savedJobID} onClick={setSavedJob}>
                                <Octicon icon={job.savedJobID > 0 ? Heart : HeartOutline} size="medium" />
                                <span className="sr-only">{job.savedJobID > 0 ? 'Unsave' : 'Save'}</span>
                            </button>
                        </Col>
                    </Row>
                    <small className="d-block text-muted mb-2">
                        {job.locations.map((l, index) =>
                            <span key={l.id}>
                                <span>{l.location}</span>
                                {index < job.locations.length - 1
                                    ? <span>,</span>
                                    : <span></span>}
                            </span>
                        )}
                    </small>

                </div>

                <div hidden={job.isAcceptingApplications}>
                    <h5 className="text-muted">This job is no longer accepting applications.</h5>
                </div>


                <div className="mb-3 small">
                    <div dangerouslySetInnerHTML={{
                        __html: job.postHTML
                    }}></div>
                </div>

                <div className="mb-3">
                    <Link className="btn btn-secondary" to="/oauth" hidden={loggedIn || !job.isAcceptingApplications}>
                        <Octicon icon={SignIn} size="small" />&nbsp;Sign in to apply
                    </Link>

                    <Link className="btn btn-primary" to={'/apply/:id'.replace(':id', job.id)} hidden={job.userHasActiveApplication || !loggedIn}>
                        <Octicon icon={Pencil} size="small" />&nbsp;Apply now
                    </Link>

                    <Link className="btn btn-success" hidden={!job.userHasActiveApplication} to={'/account'} >
                        <Octicon icon={History} size="small" />&nbsp;You have an active application.
                    </Link>
                </div>

                <div className="mb-3 small">
                    <p className="h6 mb-2">
                        Requirements
                    </p>
                    <ol className="fa-ul list-unstyled">
                        {job.requirements.map((r, index) =>
                            <li className="mb-1" key={index}>
                                <Octicon icon={ChevronRight} />
                                <strong className="mb-2 ml-2">{r.name}</strong>
                                <small className="d-block ml-4">{r.description}</small>
                            </li>
                        )}
                    </ol>
                </div>


                <div className="mb-3 small">
                    <p className="h6 mb-2">
                        Responsibilities
                    </p>
                    <ol className="list-unstyled">
                        {job.responsibilities.map((r, index) =>
                            <li className="mb-1" key={index}>
                                <Octicon icon={ChevronRight} />
                                <strong className="mb-2 ml-2">{r.name}</strong>
                                <small className="d-block ml-4">{r.description}</small>
                            </li>
                        )}
                    </ol>
                </div>

                <div className="mb-3">
                    <Row>
                        <Col xs="4">
                            <strong className="d-block">
                                Job Type
                            </strong>
                            <small className="d-block text-muted">
                                {job.jobTypeName}
                            </small>
                        </Col>
                        <Col md="4">
                            <strong className="d-block">
                                Seniority Level
                            </strong>
                            <small className="d-block text-muted">
                                {job.seniorityLevelName}
                            </small>
                        </Col>
                        <Col md="4">
                            <strong className="d-block">
                                Remote
                            </strong>
                            <small className="d-block text-muted">
                                {job.allowRemote === null ? 'Not specified' : job.allowRemote === true ? 'Yes' : 'No'}
                            </small>
                        </Col>
                        <Col md="4">
                            <strong className="d-block">
                                Team Size
                            </strong>
                            <small className="d-block text-muted">
                                {job.teamSize}
                            </small>
                        </Col>
                        <Col md="4">
                            <strong className="d-block">
                                Salary
                            </strong>
                            <small className="d-block text-muted">
                                {job.minSalary} - {job.maxSalary}
                            </small>
                        </Col>
                        <Col md="4">
                            <strong className="d-block">
                                Travel
                            </strong>
                            <small className="d-block text-muted">
                                {job.travel}
                            </small>
                        </Col>
                    </Row>
                </div>


                <div className="mb-3 small text-muted">
                    <p className="h6 mb-2">
                        Employer Benefits
                    </p>
                    <ol className="list-unstyled">
                        {job.benefits.map((r, index) =>
                            <li className="mb-1" key={index}>
                                <Octicon icon={ChevronRight} />
                                <strong className="mb-2 ml-2">{r.name}</strong>
                                <small className="d-block ml-4">{r.description}</small>
                            </li>
                        )}
                    </ol>
                </div>
            </div>
            )
    }

    static renderSkills(skills) {
        return (
            <AuthConsumer>
                {({ auth }) => (
                    <div>
                        {skills.map((s, index) =>
                            <div className={auth && s.userHasSkill ? 'text-success' : 'text-dark'} key={index}>
                                {<Octicon icon={auth && s.userHasSkill ? Check : Code} />}&nbsp;
                                {s.skillName}
                            </div>
                        )}
                        <div hidden={!auth}>
                            <small className="text-muted">You have {skills.filter(s => s.userHasSkill).length} out of {skills.length} skills.</small>
                        </div>
                    </div>
                )}
            </AuthConsumer>
        )
    }

    static renderMethods(methods) {
        return (
            <ul className="list-unstyled">
                {methods.map((r, index) =>
                    <li className="mb-1" key={index}>
                        <Octicon icon={Pin} />
                        <strong className="mb-2 ml-2">{r.name}</strong>
                        <small className="d-block ml-4">{r.description}</small>
                    </li>
                )}
            </ul>
        )
    }

}

Job.contextType = AuthContext