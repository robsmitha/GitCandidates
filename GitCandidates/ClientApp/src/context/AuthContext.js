import React, { Component } from 'react';
import { authService } from './../services/auth.service'

const AuthContext = React.createContext();

class AuthProvider extends Component {

    state = {
        auth: authService.appUserValue !== null
            && authService.appUserValue.auth
    }

    constructor(props) {
        super(props)
    }

    componentDidMount() {
        authService.appUser.subscribe(x => {
            if (x !== null && x.auth) {
                authService.authorize()
                    .then(data => {
                        let auth = data !== null && data.auth;
                        if (!auth) authService.clearAppUser();
                        this.setState({ auth: auth })
                    })
            }
        })
    }

    
    signIn = () => {
        this.setState({ auth: true })
    }

    signOut = () => {
        authService.signOut();
        this.setState({ auth: false })
    }

    render() {
        return (
            <AuthContext.Provider value={{
                auth: this.state.auth,
                signOut: this.signOut,
                signIn: this.signIn
            }}>
                {this.props.children}
            </AuthContext.Provider>
        )
    }
}

const AuthConsumer = AuthContext.Consumer

export { AuthProvider, AuthConsumer, AuthContext }