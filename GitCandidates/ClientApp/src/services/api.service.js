export async function get(url) {
    return fetch(url)
        .then(handleResponse)
}

export async function post(url, data) {
    const request = {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }
    return fetch(url, request)
        .then(handleResponse)
}

async function handleResponse(response) {
    let data = null
    if (response.ok) return response.json();
    if (!response.ok && response.status === 400) {
        let errors = ''
        for (let d in data) errors += data[d] + '\n'
        return errors;
    }
    else {
        console.log('an unknow error occurred')
    }
    return data;
}