import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import validate from './../helpers/Validate'
import TextInput from './../helpers/TextInput'
import Octicon, { Telescope, Location } from '@primer/octicons-react';
import { jobService } from './../services/job.service'

export class Home extends Component {

    static displayName = Home.name;
    constructor(props) {
        super(props)
        this.state = {
            formIsValid: false,
            formControls: {
                keyword: {
                    value: '',
                    placeholder: 'Enter job title',
                    label: 'Job title',
                    valid: true,
                    touched: false,
                    errors: []
                },
                location: {
                    value: '',
                    placeholder: 'Enter location',
                    label: 'Location',
                    valid: true,
                    touched: false,
                    errors: []
                }
            },
            jobs: [],
            loading: true
        }
    }

    componentDidMount() {
        jobService.getJobs({ })
            .then(data => this.setState({
                loading: false,
                jobs: data
            }))
    }

    changeHandler = event => {
        const name = event.target.name;
        const value = event.target instanceof HTMLInputElement && event.target.getAttribute('type') == 'checkbox'
            ? event.target.checked
            : event.target.value;
        this.handleChange(name, value)
    }

    handleChange = (name, value) => {
        const updatedControls = {
            ...this.state.formControls
        };

        const updatedFormElement = {
            ...updatedControls[name]
        };

        updatedFormElement.value = value;
        updatedFormElement.touched = true;
        updatedFormElement.errors = validate(value, updatedFormElement.validationRules, updatedFormElement.placeholder);
        updatedFormElement.valid = updatedFormElement.errors === undefined || updatedFormElement.errors === null || updatedFormElement.errors.length === 0;
        updatedControls[name] = updatedFormElement;
        let formIsValid = true;
        for (let inputIdentifier in updatedControls) {
            formIsValid = updatedControls[inputIdentifier].valid && formIsValid;
        }
        this.setState({
            formControls: updatedControls,
            formIsValid: formIsValid
        })
    }

    submitHandler = event => {
        event.preventDefault()
        let data = {
            keyword: this.state.formControls.keyword.value,
            location: this.state.formControls.location.value
        }
        jobService.getJobs(data)
            .then(data => this.setState({
                loading: false,
                jobs: data
            }))
    }

    render() {
        let contents = this.state.loading
            ? Home.renderLoading()
            : Home.renderJobs(this.state.jobs)
        return (
            <div className="mb-4">
                <header className="bg-light py-3 mb-5">
                    <div className="container h-100">
                        <div className="row h-100 align-items-center">
                            <div className="col-md-8 mx-auto">
                                <div className="my-5">
                                    <h1 className="display-4">
                                        <Octicon icon={Telescope} size="large" />
                                        GitCandidates
                                    </h1>
                                    <p className="text-dark-50">
                                        Finding the perfect job octo be easy.
                                    </p>
                                    <form onSubmit={this.submitHandler}>
                                        <div className="form-row">
                                            <div className="col-12 col-md-5 mb-2 mb-md-0">
                                                <TextInput name="keyword"
                                                    placeholder={this.state.formControls.keyword.placeholder}
                                                    label={this.state.formControls.keyword.label}
                                                    value={this.state.formControls.keyword.value}
                                                    onChange={this.changeHandler}
                                                    touched={this.state.formControls.keyword.touched ? 1 : 0}
                                                    valid={this.state.formControls.keyword.valid ? 1 : 0}
                                                    errors={this.state.formControls.keyword.errors} />
                                            </div>
                                            <div className="col-12 col-md-5 mb-2 mb-md-0">
                                                <TextInput name="location"
                                                    placeholder={this.state.formControls.location.placeholder}
                                                    label={this.state.formControls.location.label}
                                                    value={this.state.formControls.location.value}
                                                    onChange={this.changeHandler}
                                                    touched={this.state.formControls.location.touched ? 1 : 0}
                                                    valid={this.state.formControls.location.valid ? 1 : 0}
                                                    errors={this.state.formControls.location.errors} />
                                            </div>
                                            <div className="col-12 col-md-2">
                                                <br />
                                                <button type="submit" className="btn btn-block mt-2 btn-success">
                                                    Search
                                                </button>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </header>
                <div className="container">
                    <h2 className="h3 border-bottom pb-2 mb-4">
                        <Octicon icon={Location} size="medium" />&nbsp;
                        Jobs near you
                    </h2>
                    {contents}
                </div>
            </div>
        );
    }
    static renderLoading() {
        return (
            <div className="vh-100 d-flex align-items-stretch py-5">
                <div className="container">
                    <div className="row">
                        <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                            <div className="text-center">
                                <p className="lead">
                                    <span className="spinner-grow" role="status">
                                        <span className="sr-only">Loading...</span>
                                    </span>
                                    Loading opportunities..
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            )
    }
    static renderJobs(jobs) {
        return (
            <div className="row">
                {jobs.map(j =>
                    <div className="col-md-3 mb-4" key={j.id}>
                        <Link to={'/job/:id'.replace(':id', j.id)} className="text-decoration-none">
                            <div className="card h-100">
                                <div className="card-body d-flex flex-column">
                                    <h5 className="card-title">{j.name}</h5>
                                    <strong className="d-block card-text text-dark mb-1">
                                        {j.companyGitHubLogin}
                                    </strong>
                                    <small className="d-block text-muted">
                                        {j.locations.map((l, index) =>
                                            <span key={l.id}>
                                                <span>{l.location}</span>
                                                {index < j.locations.length - 1
                                                    ? <span>,</span>
                                                    : <span></span>}
                                            </span>
                                        )}
                                    </small>
                                    <small className="text-muted mt-auto">
                                        {j.posted}
                                    </small>
                                </div>
                            </div>
                        </Link>
                    </div>
                )}
            </div>
            )
    }
}
