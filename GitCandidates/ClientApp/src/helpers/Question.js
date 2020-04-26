import React from 'react';

const TYPES = {
    RADIO: 'radio',
    TEXT: 'text',
    SELECT: 'select',
}


const Question = props => {

    let className = props.className === undefined ? '' : props.className
    switch (props.type) {
        case TYPES.RADIO:
            className += ' custom-control-input'
            break;
        default:
            className += ' form-control';
            break;
    }
    if (props.touched && !props.valid) className += ' is-invalid';
    let hidden = props.hidden === undefined ? false : props.hidden;
    let label = props.label === undefined ? props.name : props.label;
    let responses = props.responses === undefined ? [] : props.responses
    let errors = props.errors === undefined ? [] : props.errors

    let options = {
        name: props.name,
        className: className,
        hidden: hidden,
        label: label,
        responses: responses,
        errors: errors
    }

    return (
        <div>
            {generateQuestion(options, props)}
        </div>
    )
}

function generateQuestion(options, props) {
    switch (props.type) {
        case TYPES.RADIO: return renderRadio(options, props);
    }
    return renderText(options, props)
}


function renderText(options, props) {
    return (
        <div className="form-group" hidden={options.hidden}>
            <label htmlFor={options.name} className="font-weight-light">{options.label}</label>
            <input className={options.className} {...props} />
            {options.errors.map(e =>
                <div key={e} className="text-danger">
                    {e}
                </div>
            )}
        </div>
    )
}

function renderRadio(options, props) {
    return (
        <div className="form-group" hidden={options.hidden}>
            <label htmlFor={options.name} className="font-weight-light">{options.label}</label>
            {options.responses.map((r, index) =>
                <div key={r.id} className="custom-control custom-radio">
                    <input id={'r_' + r.id} {...props} className={options.className} />
                    <label className="custom-control-label" htmlFor={'r_' + r.id}>{r.answer}</label>
                </div>
            )}
            {options.errors.map(e =>
                <div key={e} className="text-danger">
                    {e}
                </div>
            )}
        </div>
        )
}

export default Question
