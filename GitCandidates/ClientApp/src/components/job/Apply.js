import React, { Component } from 'react';
import { Redirect, Link, useHistory} from 'react-router-dom';
import { jobService } from '../../services/job.service';
import { AuthConsumer } from './../../context/AuthContext';
import Octicon, { Pencil } from '@primer/octicons-react';
import Question from '../../helpers/Question';
import validate from './../../helpers/Validate'; 

export class Apply extends Component {

    constructor(props) {
        super(props)
        this.state = {
            loading: true,
            id: this.props.match.params.id,
            application: null,
            formControls: null,
            formIsValid: false
        }
    }

    componentDidMount() {
        jobService.getJobApplication(this.state.id)
            .then(data => {
                let formControls = {};
                data.questions.forEach(q => {
                    formControls[`q_${q.id}`] = {
                        value: '',
                        placeholder: q.questionPlaceholder,
                        label: q.questionLabel,
                        valid: false,
                        touched: false,
                        validationRules: q.validationRules,
                        type: q.questionResponseTypeInput,
                        responses: q.responses,
                        errors: [],
                        jacId: q.id
                    }
                })

                this.setState({
                    loading: false,
                    application: data,
                    formControls: formControls,
                    formIsValid: formControls.length == 0
                })
            })
    }

    changeHandler = event => {
        const name = event.target.name;
        const value = event.target instanceof HTMLInputElement
            && (event.target.getAttribute('type') == 'checkbox' || event.target.getAttribute('type') == 'radio')
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
        let responses = []
        Object.keys(this.state.formControls)
            .map((name, index) => {
                let data = this.state.formControls[name];
                responses.push({
                    jobApplicationQuestionID: data.jacId,
                    response: data.value
                })
            })

        let data = {
            jobId: Number(this.state.id),
            responses: responses
        }
        console.log(data)
        jobService.createJobApplication(data)
            .then(data => {
                if (data) {
                    this.props.history.push('/account')
                }
            })
    }

    render() {
        let contents = this.state.loading
            ? Apply.renderLoading()
            : this.renderApplication()
        return (
            <div>
                <AuthConsumer>
                    {({ auth }) => (
                        <div>
                            {!auth
                                ? <Redirect to={'/job/:id'.replace(':id', this.state.id)} />
                                : contents}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        );
    }

    renderApplication() {
        const application = this.state.application;
        const jobid = this.state.id
        return (

            <div className="d-flex align-items-stretch py-5">
                <div className="container">
                    <div className="row">
                        <div className="col-sm-9 col-md-7 col-lg-5 mx-auto">
                            <div className="text-center">
                                <Octicon icon={Pencil} size="medium" />
                                <Link to={'/job/:id'.replace(':id', jobid)} className="text-decoration-none">
                                    <h1 className="h3">
                                        {application.jobName}
                                    </h1>
                                </Link>
                                <strong className="d-block">
                                    {application.companyGitHubLogin}
                                </strong>
                                <hr className="w-25" />

                                <form onSubmit={this.submitHandler}>
                                    <div hidden={this.state.formControls.length === 0}>
                                        <div className="list-group">
                                            <h4 className="list-group-item rounded-0 lead">
                                                Please answer the following question{application.questions.length === 1 ? '' : 's'}
                                            </h4>
                                            {Object.keys(this.state.formControls).map((name, index) =>
                                                <div className="list-group-item" key={name}>
                                                    <Question
                                                        label={this.state.formControls[name].label}
                                                        placeholder={this.state.formControls[name].placeholder}
                                                        type={this.state.formControls[name].type}
                                                        responses={this.state.formControls[name].responses}
                                                        errors={this.state.formControls[name].errors}
                                                        validationrules={this.state.formControls[name].validationRules}
                                                        questionid={this.state.formControls[name].questionId}
                                                        name={name}
                                                        onChange={this.changeHandler}
                                                        touched={this.state.formControls[name].touched ? 1 : 0}
                                                        valid={this.state.formControls[name].valid ? 1 : 0}
                                                    />
                                                </div>
                                            )}
                                            <button type="submit" className="list-group-item list-group-item-success list-group-item-action" disabled={!this.state.formIsValid}>
                                                Submit application
                                            </button>
                                            <Link to={'/job/:id'.replace(':id', jobid)} className="list-group-item list-group-item-action rounded-0">
                                                Discard application
                                            </Link>
                                        </div>
                                    </div>
                                </form>
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
                                    Loading job application..
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}