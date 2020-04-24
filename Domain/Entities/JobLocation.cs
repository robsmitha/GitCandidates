using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class JobLocation : BaseType
    {
        /// <summary>
        /// Job that address belongs to
        /// </summary>
        public int JobID { get; set; }
        [ForeignKey("JobID")]
        public Job Job { get; set; }

        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public static class JobLocationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="mi"></param>
        /// <returns></returns>
        public static bool WithinMiles(this JobLocation @this, double? lat1, double? lon1, double mi)
        {
            if (lat1 == null || lon1 == null) return true;
            return @this.HaversineDistance(lat1, lon1) <= mi;
        }

        /// <summary>
        /// Use Haversine Distance formula to calculate a distance between two points
        /// </summary>
        /// <param name="this"></param>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <returns></returns>
        public static double HaversineDistance(this JobLocation @this, double? lat1, double? lon1)
        {
            var lat2 = @this.Latitude;
            var lon2 = @this.Longitude;
            var R = 3960;   //miles
            static double ToRadians(double angle) => Math.PI * angle / 180.0;
            var dLat = ToRadians(lat2 - lat1.Value);
            var dLon = ToRadians(lon2 - lon1.Value);
            lat1 = ToRadians(lat1.Value);
            lat2 = ToRadians(lat2);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1.Value) * Math.Cos(lat2);
            var c = 2 * Math.Asin(Math.Sqrt(a));
            return R * c;

        }
    }
}
