import React, { Component } from 'react';
import { Redirect, Link } from 'react-router-dom';
import { jobService } from '../../services/job.service';
import { AuthConsumer } from './../../context/AuthContext';
import Octicon, { Briefcase } from '@primer/octicons-react';

export class Job extends Component {

    constructor(props) {
        super(props)
        this.state = {
            loading: true,
            id: this.props.match.params.id,
            job: null
        }
    }

    componentDidMount() {
        jobService.getJob(this.state.id)
            .then(data => this.setState({
                loading: false,
                job: data
            }))
    }

    render() {
        let contents = this.state.loading
            ? Job.renderLoading()
            : Job.renderJob(this.state.job)
        return (
            <div>
                {contents}
            </div>
        );
    }

    static renderJob(job) {
        return (
            
            <div className="d-flex align-items-stretch py-5">
                <div className="container">
                    <div className="row">
                        <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                            <div className="text-center">
                                <Octicon icon={Briefcase} size="medium" />
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
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-10 mx-auto">
                            <p>
                                {job.description}
                            </p>
                        </div>
                    </div>
                    <div className="text-center mt-md-4 mt-3">
                        <h5>
                            Is this a good fit?
                        </h5>

                        <AuthConsumer>
                            {({ auth }) => (
                                <div>
                                    {!auth
                                        ? <Link to="/oauth">Sign in to apply.</Link>
                                        : <Link to={'/apply/:id'.replace(':id', job.id)}>Yes, apply now!</Link>}
                                </div>
                            )}
                        </AuthConsumer>
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
                                    Loading opportunity..
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}