using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.Models
{
    public class GeoLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Altitude { get; set; }
        public bool HasAltitude { get; set; }
        public string Locality { get; set; }

        public GeoLocation() { }
        public GeoLocation(double latitude, double longitude, double? altitude = null)
        {
            Latitude = latitude;
            Longitude = longitude;
            if (altitude == null)
                HasAltitude = false;
            else HasAltitude = true;
            Altitude = altitude;
        }
    }
}
