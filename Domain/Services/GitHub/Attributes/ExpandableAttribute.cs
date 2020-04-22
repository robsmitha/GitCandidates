using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Domain.Services.GitHub.Attributes
{
    public class ExpandableAttribute : Attribute
    {
        /// <summary>
        /// Entity to expand on (i.e. "repo", "organziation")
        /// </summary>
        public string Expand { get; set; }

        /// <summary>
        /// the expected result type of the expansion
        /// </summary>
        public Type Result { get; set; }

        /// <summary>
        /// the property the result will map to
        /// </summary>
        public string MapTo { get; set; }

        public class MetaData
        {
            public string expand { get; set; }
            public Type genericType { get; set; }
            public object value { get; set; }
            public string mapTo { get; set; }
        }
        /// <summary>
        /// Get expandable properties from an object by filtering class properties
        /// </summary>
        /// <param name="obj">object whose attributes to check</param>
        /// <returns></returns>
        public static Dictionary<string, MetaData> MapFromObject(object obj)
        {
            return obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(prop => IsDefined(prop, typeof(ExpandableAttribute)))
                    .Select(prop =>
                    {
                        var attr = prop.GetCustomAttribute(typeof(ExpandableAttribute)) as ExpandableAttribute;
                        return new MetaData
                        {
                            expand = attr.Expand,
                            genericType = attr.Result,
                            value = prop.GetValue(obj, null),
                            mapTo = attr.MapTo
                        };
                    }).ToDictionary(data => data.expand, data => data);
        }
    }
}
