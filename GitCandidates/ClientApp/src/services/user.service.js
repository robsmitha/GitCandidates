import { post, get } from './api.service';

export const userService = {
    getUser,
    getJobApplications,
    withdrawApplication,
    setSavedJob,
    getSavedJobs
};

function getUser() {
    return get(`users/getuser`)
}

function getJobApplications() {
    return get(`users/getJobApplications`)
}

function getSavedJobs() {
    return get(`users/getSavedJobs`)
}

function withdrawApplication(data) {
    return post(`users/withdrawApplication`, data)
}

function setSavedJob(data) {
    return post(`users/setSavedJob`, data)
}
