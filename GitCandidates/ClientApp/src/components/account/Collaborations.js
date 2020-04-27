import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import Loading from '../../helpers/Loading';
import { userService } from '../../services/user.service';
import { AuthConsumer } from './../../context/AuthContext';
import { NavAccountMenu } from './NavAccountMenu';
import GitHubUser from '../../helpers/GitHubUser';

export class Collaborations extends Component {
    constructor(props) {
        super(props)
        this.state = {
            userLoading: true,
            user: null,
            collaborations: []
        }
    }
    componentDidMount() {
        this.populateUser()
    }

    populateUser = () => {
        userService.getUser()
            .then(data => this.setState({ user: data }))
    }

    render() {
        return (
            <div>
                <AuthConsumer>
                    {({ auth }) => (
                        <div>
                            {!auth
                                ? <Redirect to="/oauth" />
                                : this.renderLayout()}
                        </div>
                    )}
                </AuthConsumer>
            </div>
        );
    }

    renderLayout() {
        const { collaborations, user } = this.state
        return (
            <div>
                <div className="py-3">
                    <div className="container">
                        <div className="row">
                            <div className="col-lg-2 col-md-3 col-sm-4 col-12">
                                <div className="text-center">
                                    {user === undefined || user === null
                                        ? <Loading />
                                        : <GitHubUser user={user.gitHubUser} />}
                                </div>
                            </div>
                            <div className="col">
                                <NavAccountMenu />
                                <div>
                                    <h4 className="pl-3">Collaborations</h4>
                                    {collaborations === null
                                        ? <Loading message="Loading collaborations" />
                                        : Collaborations.renderCollaborations(collaborations)}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    static renderCollaborations(collaborations) {
        return (
            <div>
                <div hidden={collaborations.length > 0}>
                    No collaborations to display.
                </div>
            </div>
            )
    }
}