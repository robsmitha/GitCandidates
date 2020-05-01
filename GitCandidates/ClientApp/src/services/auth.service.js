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
    getClient,
    appUser: appUserSubject.asObservable(),
    get appUserValue() { return appUserSubject.value }
};

async function getClient() {
    let ip = await fetch('https://api.ipify.org?format=json');
    let data = await ip.json();
    if (data.ip) {
        let geo = await fetch(`https://api.hackertarget.com/geoip/?q=${data.ip}`);
        let text = await geo.text();
        var client = {}
        var lines = text.split('\n')
        for (let i = 0; i < lines.length; i++) {
            var kv = lines[i].split(':');
            var k = kv[0].replace(/\s+/g, '')
            var v = kv[1].trim()
            client[k] = v;
        }
        return client;
    }
}


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