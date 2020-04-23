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
    }
}
