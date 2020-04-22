import { post, get } from './api.service';

export const userService = {
    getUser
};

function getUser() {
    return get(`users/getuser`)
}
