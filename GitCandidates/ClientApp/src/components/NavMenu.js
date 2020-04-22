import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { AuthConsumer } from './../context/AuthContext'
import Octicon, { Telescope, Home, SignIn, SignOut, Person } from '@primer/octicons-react'

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    componentDidMount() {
        this.configureUI()
    }

    configureUI() {
        var navbar = document.querySelector('.main-nav').clientHeight;
        document.querySelector('.header').style = 'padding-top: ' + navbar + 'px';
    }

    render() {
        return (
            <header className="header">
                <AuthConsumer>
                    {({ auth }) => (
                        <header>
                            <Navbar className="main-nav navbar-expand-sm navbar-toggleable-sm box-shadow fixed-top bg-dark border-bottom shadow-sm navbar-dark">
                                <Container>
                                    <NavbarBrand tag={Link} to="/">
                                        <Octicon icon={Telescope} size="medium" />&nbsp;
                                        GitCandidates
                                    </NavbarBrand>
                                    <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                                    <Collapse isOpen={!this.state.collapsed} navbar>
                                        <ul className="navbar-nav">
                                            <NavItem>
                                                <NavLink tag={Link} to="/">
                                                    <Octicon icon={Home} size="small" />&nbsp;
                                                    Home
                                                </NavLink>
                                            </NavItem>
                                        </ul>
                                        <ul className="navbar-nav ml-auto">
                                            <NavItem hidden={auth}>
                                                <NavLink tag={Link} to="/oauth">
                                                    <Octicon icon={SignIn} size="small" />&nbsp;
                                                    Sign in
                                                </NavLink>
                                            </NavItem>
                                            <NavItem hidden={!auth}>
                                                <NavLink tag={Link} to="/account">
                                                    <Octicon icon={Person} size="small" />&nbsp;
                                                    Account
                                                </NavLink>
                                            </NavItem>
                                            <NavItem hidden={!auth}>
                                                <NavLink tag={Link} to="/sign-out">
                                                    <Octicon icon={SignOut} size="small" />&nbsp;
                                                    Sign out
                                                </NavLink>
                                            </NavItem>
                                        </ul>
                                    </Collapse>
                                </Container>
                            </Navbar>
                        </header>
                    )}
                </AuthConsumer>
            </header>
        );
    }
}
