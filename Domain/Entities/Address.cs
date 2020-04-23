using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Address : BaseEntity
    {
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
