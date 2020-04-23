using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Job : BaseType
    {
        public string Company { get; set; }
        public string Location { get; set; }
        public double Latitide { get; set; }
        public double Longitude { get; set; }
    }
}
