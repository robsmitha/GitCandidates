import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { AuthConsumer } from './../context/AuthContext'

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
                            <Navbar className="main-nav navbar-expand-sm navbar-toggleable-sm box-shadow fixed-top bg-white border-bottom shadow-sm navbar-light">
                                <Container>
                                    <NavbarBrand tag={Link} to="/">
                                        GitCandidates
                                    </NavbarBrand>
                                    <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                                    <Collapse isOpen={!this.state.collapsed} navbar>
                                        <ul className="navbar-nav">
                                            <NavItem>
                                                <NavLink tag={Link} to="/">Home</NavLink>
                                            </NavItem>
                                        </ul>
                                        <ul className="navbar-nav ml-auto">
                                            <NavItem hidden={auth}>
                                                <NavLink tag={Link} to="/oauth">
                                                    Sign in with GitHub
                                                </NavLink>
                                            </NavItem>
                                            <NavItem hidden={!auth}>
                                                <NavLink tag={Link} to="/account">
                                                    Account
                                                </NavLink>
                                            </NavItem>
                                            <NavItem hidden={!auth}>
                                                <NavLink tag={Link} to="/sign-out">
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
