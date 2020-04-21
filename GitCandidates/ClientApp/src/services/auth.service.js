import { BehaviorSubject } from 'rxjs';
import { post, get } from './api.service';

const _appUserKey = 'appUser';

const appUserSubject = new BehaviorSubject(JSON.parse(localStorage.getItem(_appUserKey)));

export const authService = {
    authorize,
    signOut,
    clearAppUser,
    gitHubOAuthUrl,
    gitHubOAuthCallback,
    appUser: appUserSubject.asObservable(),
    get appUserValue() { return appUserSubject.value }
};



function gitHubOAuthUrl(login) {
    return get(`auth/GitHubOAuthUrl/${login}`)
}

function gitHubOAuthCallback(data) {
    return post(`auth/GitHubOAuthCallback`, data)
        .then(data => {
            setAppUser(data)
            return true;
        })
}

function authorize() {
    return post(`auth/authorize`)
}

function signOut() {
    post(`auth/signout`)
        .then(data => {
            if (data) clearAppUser();
        });
}

function setAppUser(data) {
    localStorage.setItem('appUser', JSON.stringify(data))
}

function clearAppUser() {
    // remove user from local storage to log user out
    localStorage.removeItem(_appUserKey);
    appUserSubject.next(null);
}