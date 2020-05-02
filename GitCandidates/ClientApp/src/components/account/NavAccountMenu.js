import React, { Component } from 'react';
import { Link, NavLink } from 'react-router-dom';
import Octicon, { Star, Gear, InternalRepo, Clippy, Heart } from '@primer/octicons-react';

export class NavAccountMenu extends Component {
    constructor(props) {
        super(props)
    }

    render() {
        return (
            <nav className="nav nav-tabs nav-fill mb-2">
                <NavLink activeClassName="active" to="/account" className="nav-item nav-link">
                    <Octicon icon={Clippy} size="small" />&nbsp;
                    Applications
                </NavLink>
                <NavLink activeClassName="active" to="/saved-jobs" className="nav-item nav-link">
                    <Octicon icon={Heart} size="small" />&nbsp;
                    Saved Jobs
                </NavLink>
                <NavLink activeClassName="active" to="/collaborations" className="nav-item nav-link">
                    <Octicon icon={InternalRepo} size="small" />&nbsp;
                    Collaborations
                </NavLink>
                <NavLink activeClassName="active" to="/settings" className="nav-item nav-link">
                    <Octicon icon={Gear} size="small" />&nbsp;
                    Settings
                </NavLink>
            </nav>
        );
    }
}