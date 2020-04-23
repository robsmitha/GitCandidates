import { post, get } from './api.service';

export const jobService = {
    getJobs,
    getJob
};

function getJobs(data) {
    return post(`jobs/getjobs`, data)
}

function getJob(id) {
    return get(`jobs/getjob/${id}`)
}
