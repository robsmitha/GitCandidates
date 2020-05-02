import React, { Component } from 'react';
import { Redirect, Link, useHistory} from 'react-router-dom';
import { jobService } from '../../services/job.service';
import { AuthConsumer } from './../../context/AuthContext';
import Octicon, { Pencil, ChevronLeft, ChevronRight, Tasklist } from '@primer/octicons-react';
import Question from '../../helpers/Question';
import validate from './../../helpers/Validate';
import Loading from './../../helpers/Loading'
import { Row, Col } from 'reactstrap';


export class Apply extends Component {

    constructor(props) {
        super(props)
        this.state = {
            jobid: this.props.match.params.id,
            job: null,
            formControls: null,
            formIsValid: false
        }
    }

    componentDidMount() {
        jobService.getJobApplication(this.state.jobid)
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
                        jacId: q.id,
                        min: q.questionMinimum,
                        max: q.questionMaximum
                    }
                })

                this.setState({
                    job: data.job,
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

        if (!this.state.formIsValid) return

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
            jobId: Number(this.state.jobid),
            responses: responses
        }

        jobService.createJobApplication(data)
            .then(data => {
                if (data === true) {
                    this.props.history.push('/account')
                }
                else {
                    console.log(data)
                }
            })
    }

    render() {
        return (
            <AuthConsumer>
                {({ auth }) => (
                    <div>
                        {!auth
                            ? <Redirect to={'/job/:id'.replace(':id', this.state.jobid)} />
                            : this.renderLayout()}
                    </div>
                )}
            </AuthConsumer>
        );
    }

    renderLayout() {
        const { job, jobid, formControls, formIsValid } = this.state
        return (
            <div className="container py-3">
                <div className="row">
                    <div className="col-md-8">
                        <h1 className="mb-1 h6 text-primary text-uppercase">
                            Questionnaire
                        </h1>
                        <h2 className="h4">
                            Some quick questions
                        </h2>
                        {job === null
                            ? <Loading message="Loading questions, please wait.." />
                            : Apply.renderApplication(formControls, formIsValid, jobid, this.changeHandler, this.submitHandler)}
                    </div>
                    <div className="col-md">
                        {job === null
                            ? <Loading />
                            : Apply.renderJob(job)
                        }
                    </div>
                </div>
            </div>
        )
    }
    static renderApplication(formControls, formIsValid, jobid, changeHandler, submitHandler) {
        return (
            <form onSubmit={submitHandler}>
                <div hidden={formControls.length === 0}>
                    <div className="mb-3">
                        {Object.keys(formControls).map((name, index) =>
                            <div className={'border-bottom '.concat(index == 0 ? 'pb-2' : 'pt-2')} key={name} >
                                <Question
                                    label={formControls[name].label}
                                    placeholder={formControls[name].placeholder}
                                    type={formControls[name].type}
                                    responses={formControls[name].responses}
                                    errors={formControls[name].errors}
                                    validationrules={formControls[name].validationRules}
                                    questionid={formControls[name].questionId}
                                    name={name}
                                    onChange={changeHandler}
                                    touched={formControls[name].touched ? 1 : 0}
                                    valid={formControls[name].valid ? 1 : 0}
                                    min={formControls[name].min}
                                    max={formControls[name].max}
                                />
                            </div>
                        )}
                    </div>
                </div>
                <Row>
                    <Col>
                        <Link to={'/job/:id'.replace(':id', jobid)} className="btn btn-light">
                            <Octicon icon={ChevronLeft} size="small" />&nbsp;Back
                        </Link>
                    </Col>
                    <Col>
                        <button type="submit" className="btn btn-primary float-right" disabled={!formIsValid}>
                            Submit&nbsp;<Octicon icon={ChevronRight} size="small" />
                        </button>
                    </Col>
                </Row>
            </form>
            )
    }
    static renderJob(job) {
        return (
            <div>
                <h4 className="mb-1 h6 text-primary text-uppercase">
                    {job.companyGitHubLogin}
                </h4>
                <h5 className="h5 mb-0">
                    {job.name}
                </h5>
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

                <Row>
                    <Col xs="6">
                        <strong className="d-block">
                            Job Type
                            </strong>
                        <small className="d-block text-muted">
                            {job.jobTypeName}
                        </small>
                    </Col>
                    <Col md="6">
                        <strong className="d-block">
                            Seniority Level
                            </strong>
                        <small className="d-block text-muted">
                            {job.seniorityLevelName}
                        </small>
                    </Col>
                    <Col md="6">
                        <strong className="d-block">
                            Remote
                            </strong>
                        <small className="d-block text-muted">
                            {job.allowRemote === null ? 'Not specified' : job.allowRemote === true ? 'Yes' : 'No'}
                        </small>
                    </Col>
                    <Col md="6">
                        <strong className="d-block">
                            Team Size
                            </strong>
                        <small className="d-block text-muted">
                            {job.teamSize}
                        </small>
                    </Col>
                    <Col md="6">
                        <strong className="d-block">
                            Salary
                            </strong>
                        <small className="d-block text-muted">
                            {job.minSalary} - {job.maxSalary}
                        </small>
                    </Col>
                    <Col md="6">
                        <strong className="d-block">
                            Travel
                            </strong>
                        <small className="d-block text-muted">
                            {job.travel}
                        </small>
                    </Col>
                </Row>
            </div>
        )
    }
}