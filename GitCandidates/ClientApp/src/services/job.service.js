import { post, get } from './api.service';

export const jobService = {
    getJobs,
    getJob,
    getJobApplication,
    createJobApplication,
    getJobSkills
};

function getJobs(data) {
    return post(`jobs/getjobs`, data)
}

function getJob(id) {
    return get(`jobs/getjob/${id}`)
}

function getJobSkills(id) {
    return get(`jobs/getJobSkills/${id}`)
}

function getJobApplication(id) {
    return get(`jobs/getJobApplication/${id}`)
}

function createJobApplication(data) {
    return post(`jobs/createJobApplication`, data)
}