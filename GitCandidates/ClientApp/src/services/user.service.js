import { post, get } from './api.service';

export const userService = {
    getUser,
    withdrawApplication
};

function getUser() {
    return get(`users/getuser`)
}

function withdrawApplication(data) {
    return post(`users/withdrawApplication`, data)
}
