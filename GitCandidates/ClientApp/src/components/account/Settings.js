import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import Loading from '../../helpers/Loading';
import { userService } from '../../services/user.service';
import { AuthConsumer } from '../../context/AuthContext';
import { NavAccountMenu } from './NavAccountMenu';
import GitHubUser from '../../helpers/GitHubUser';
import Octicon, { PrimitiveDot, PrimitiveDotStroke } from '@primer/octicons-react';

export class Settings extends Component {
    constructor(props) {
        super(props)
        this.state = {
            user: null,
            settings: null
        }
    }
    componentDidMount() {
        this.populateUser()
        this.populateSettings()
    }

    populateUser = () => {
        userService.getUser()
            .then(data => this.setState({ user: data }))
    }

    populateSettings = () => {
        userService.getSettings()
            .then(data => this.setState({ settings: data }))
    }
    skillCheckHandler = (event, index) => {
        document.querySelectorAll('.user-skill').disabled = true
        const data = {
            skillId: this.state.settings.skills[index].skill.id
        }
        userService.setUserSkill(data)
            .then(data => data ? this.populateSettings() : console.log(data))
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
                                    {settings === undefined || settings === null
                                        ? <Loading message="Loading settings" />
                                        : Settings.renderSettings(settings, this.skillCheckHandler)}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    static renderSettings(settings, skillCheckHandler) {
        return (
            <div>
                <p className="mb-1 text-muted">Configure your skills for relevant job options.</p>
                <div hidden={settings.skills.length > 0}>
                    No skills to display.
                </div>
                <div>
                    {settings.skills.map((s, index) =>
                        <div className="custom-control custom-switch" key={s.skill.id}>
                            <input
                                type="checkbox"
                                className="custom-control-input user-skill"
                                id={'skill'.concat(s.skill.id)}
                                checked={s.userHasSkill}
                                onChange={(event) => skillCheckHandler(event, index)}
                            />
                            <label className="custom-control-label" htmlFor={'skill'.concat(s.skill.id)}>{s.skill.name}</label>
                        </div>
                    )}
                </div>
            </div>
        )
    }
}