import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import Loading from '../../helpers/Loading';
import { userService } from '../../services/user.service';
import { AuthConsumer } from './../../context/AuthContext';
import { NavAccountMenu } from './NavAccountMenu';
import GitHubUser from '../../helpers/GitHubUser';

export class Settings extends Component {
    constructor(props) {
        super(props)
        this.state = {
            userLoading: true,
            user: null,
            settings: []
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
        const { settings, user } = this.state
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
                                    <h4 className="pl-3">Settings</h4>
                                    {settings === null
                                        ? <Loading message="Loading settings" />
                                        : Settings.renderSettings(settings)}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    static renderSettings(settings) {
        return (
            <div>
                <div hidden={settings.length > 0}>
                    No settings to display.
                </div>
            </div>
        )
    }
}