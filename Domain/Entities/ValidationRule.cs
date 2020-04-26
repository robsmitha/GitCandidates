using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ValidationRule : BaseType
    {
        public string Key { get; set; }
    }
    public static class ValidationRuleExtensions
    {
        const string IS_REQUIRED = "isRequired";
        const string MIN_LENGTH = "minLength";
        public static string ToMessage(this ValidationRule @this, string fieldName, string msg = null, string value = null)
        {
            return @this.Key switch
            {
                IS_REQUIRED => $"{fieldName} in required.",
                MIN_LENGTH => $"{fieldName} does not meet the minimum length of {value}.",
                _ => msg,
            };
        }
    }
}
