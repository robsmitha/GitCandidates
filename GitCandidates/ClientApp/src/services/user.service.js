import { post, get } from './api.service';

export const userService = {
    getUser,
    getJobApplications,
    withdrawApplication,
    setSavedJob,
    getSavedJobs,
    getSettings,
    setUserSkill
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

function getSettings() {
    return get(`users/getSettings`)
}

function withdrawApplication(data) {
    return post(`users/withdrawApplication`, data)
}

function setSavedJob(data) {
    return post(`users/setSavedJob`, data)
}

function setUserSkill(data) {
    return post(`users/setUserSkill`, data)
}