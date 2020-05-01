import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import validate from './../helpers/Validate'
import TextInput from './../helpers/TextInput'
import Octicon, { Telescope, Location } from '@primer/octicons-react';
import { jobService } from './../services/job.service'
import Loading from './../helpers/Loading';
import { authService } from '../services/auth.service';

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
            jobs: null,
            location: '',
            displayLocation: ''
        }
    }

    componentDidMount() {
        this.populateJobs({})
    }

    populateJobs(args) {
        jobService.searchJobs(args)
            .then(data => {
                const updatedControls = {
                    ...this.state.formControls
                }
                updatedControls.location.value = data.displayLocation;
                this.setState({
                    jobs: data.jobs,
                    displayLocation: data.displayLocation,
                    formControls: updatedControls
                })
            })
    }

    useMyLocation = event => {
        this.getLocation()
    }

    getLocation() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition((position) => {
                let args = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                }
                this.populateJobs(args)
            });
        } else {
            console.log("Geolocation is not supported by this browser.")
            this.getClient()
        }
    }

    async getClient() {
        let client = await authService.getClient();
        let args = {
            lat: Number(client.Latitude),
            lng: Number(client.Longitude)
        }
        this.populateJobs(args)
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
        const { keyword, location } = this.state.formControls;
        let args = {
            keyword: keyword.value,
            location: location.value
        }
        this.populateJobs(args)
    }

    render() {
        const { jobs, formControls, displayLocation } = this.state
        let contents = jobs == null
            ? <Loading message="Loading opportunities..." />
            : Home.renderJobs(jobs)
        return (
            <div className="mb-4">
                <header className="bg-light py-3 mb-5">
                    <div className="container h-100">
                        <div className="row h-100 align-items-center">
                            <div className="col-md-12 mx-auto">
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
                                            <div className="col-12 col-md-4 mb-2 mb-md-0">
                                                <TextInput name="keyword"
                                                    placeholder={formControls.keyword.placeholder}
                                                    label={formControls.keyword.label}
                                                    value={formControls.keyword.value}
                                                    onChange={this.changeHandler}
                                                    touched={formControls.keyword.touched ? 1 : 0}
                                                    valid={formControls.keyword.valid ? 1 : 0}
                                                    errors={formControls.keyword.errors} />
                                            </div>
                                            <div className="col-12 col-md-4 mb-2 mb-md-0">
                                                <TextInput name="location"
                                                    placeholder={formControls.location.placeholder}
                                                    label={formControls.location.label}
                                                    value={formControls.location.value}
                                                    onChange={this.changeHandler}
                                                    touched={formControls.location.touched ? 1 : 0}
                                                    valid={formControls.location.valid ? 1 : 0}
                                                    errors={formControls.location.errors} />
                                            </div>
                                            <div className="col-12 col-md-2">
                                                <br />
                                                <button type="submit" className="btn btn-block mt-2 btn-success">
                                                    Search
                                                </button>
                                            </div>
                                            <div className="col-12 col-md-2">
                                                <br />
                                                <button type="button" className="btn btn-secondary mt-2 btn-block" onClick={this.useMyLocation}>
                                                    Use My Location
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
                    <h2 className="h3 pb-2 mb-2">
                        Recent Jobs
                    </h2>
                    <div className="mb-2">
                        <Octicon icon={Location} size="small" />&nbsp;
                        <small className="text-muted">
                            {displayLocation == null || displayLocation.length == 0
                                ? <span>All locations</span>
                                : <span>{displayLocation}</span>}
                        </small>
                    </div>
                    {contents}
                </div>
            </div>
        );
    }

    static renderJobs(jobs) {
        return (
            <div>
                <div hidden={jobs.length > 0}>
                    No jobs to display. Please try a different location.
                </div>
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
            </div>
            )
    }
}
