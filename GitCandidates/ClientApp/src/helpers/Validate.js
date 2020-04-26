const RULES = {
    MIN_LENGTH: 'minLength',
    IS_REQUIRED: 'isRequired',
    IS_EMAIL: 'isEmail',
    MAX_LENGTH: 'maxLength',
    IS_PHONE: 'isPhone',
}

const validate = (value, rules, label) => {
    if (rules === undefined || rules === null) return []
    let errorMessages = []
    rules.forEach(data => {
        let ruleValue = data.validationRuleValue
        let message = data.validationMessage
        let rule = data.validationRuleKey
        console.log(rule)
        switch (rule) {
            case RULES.IS_REQUIRED:
                if (ruleValue && !requiredValidator(value)) {
                    let err = message
                        ? message
                        : `${label} is required.`
                    errorMessages.push(err);
                }
                break;
            case RULES.MIN_LENGTH:
                if (!minLengthValidator(value, ruleValue)) {
                    if (value.length > 0) {
                        let err = message
                            ? message
                            : `The minimum length for ${label} is ${ruleValue} characters.`
                        errorMessages.push(err);
                    }
                }
                break;
            case RULES.MAX_LENGTH:
                if (!maxLengthValidator(value, ruleValue)) {
                    let err = message
                        ? message
                        : `The maximum length for ${label} is ${ruleValue} characters.`
                    errorMessages.push(err);
                }
                break;
        }
    })
    return errorMessages;
}


/**
 * maxLength Val
 * @param  value 
 * @param  maxLength
 * @return          
 */
const maxLengthValidator = (value, maxLength) => {
    return value.length <= maxLength;
}

/**
 * minLength Val
 * @param  value 
 * @param  minLength
 * @return          
 */
const minLengthValidator = (value, minLength) => {
    return value.length >= minLength;
}

/**
 * Check to confirm that feild is required
 * 
 * @param  value 
 * @return       
 */
const requiredValidator = value => {
    return value.trim() !== '';
}

/**
 * Email validation
 * 
 * @param value
 * @return 
 */
const emailValidator = value => {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(value).toLowerCase());
}


export default validate;