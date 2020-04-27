import React from 'react';

const GitHubUser = props => {
    const { user } = props 
    return (
        <div hidden={props.hidden}>
            <img className="img-fluid w-50" src={props.user.avatar_url} loading="lazy" />
            <strong className="d-block">{user.name}</strong>
            <a href={user.html_url} className="d-block text-decoration-none font-weight-bold" target="_blank">
                {user.login}
            </a>
            <hr className="w-25 my-1" />
        </div>
    )
}

export default GitHubUser
