import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class Footer extends Component {
    static displayName = Footer.name;

    constructor(props) {
        super(props)
    }
    componentDidMount() {

    }
    render() {
        return (
            <div>
                <footer className="bg-light pt-4 pt-md-5 border-top">
                    <div className="container">
                        <div className="row">
                            <div className="col-12 col-md">
                                <small className="mb-3 text-muted">
                                    GitCandidates&copy;{new Date().getFullYear()}
                                </small>
                            </div>
                            <div className="col-6 col-md">
                                <h5>Jobs</h5>
                                <ul className="list-unstyled text-small">
                                    <li><Link className="text-muted" to="/">Find a job</Link></li>
                                </ul>
                            </div>
                            <div className="col-6 col-md">
                                <h5>About</h5>
                                <ul className="list-unstyled text-small">
                                    <li><Link className="text-muted" to="/">How it works</Link></li>
                                </ul>
                            </div>
                            <div className="col-6 col-md">
                                <h5>Employers</h5>
                                <ul className="list-unstyled text-small">
                                    <li><Link className="text-muted" to="/">Post a job</Link></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </footer>
            </div>
        )
    }
}