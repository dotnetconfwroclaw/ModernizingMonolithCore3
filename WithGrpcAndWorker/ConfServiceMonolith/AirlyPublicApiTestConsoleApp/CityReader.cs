using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AirlyPublicApiTestConsoleApp
{
    public class CityLocation
    {
        public CityLocation(string cityName, double longitude, double latitude)
        {
            CityName = cityName;
            Longitude = longitude;
            Latitude = latitude;
        }

        public string CityName { get; }
        public double Longitude { get; }
        public double Latitude { get; }

        public string LongitudeText => Longitude.ToString("F6", CultureInfo.InvariantCulture);
        public string LatitudeText => Latitude.ToString("F6", CultureInfo.InvariantCulture);
    }
    public class CityReader
    {
        public List<CityLocation> GetCityLocations(string[] lines)
        {
            return lines.Select(GetCityFromLine).ToList();
        }

        private CityLocation GetCityFromLine(string line)
        {
            // 52°07'N
            var cityName = line.Substring(0, 24);
            var longitudeText = line.Substring(24, 5);
            var latitudeText = line.Substring(39, 5);
            var longitude = GetGeographicValue(longitudeText);
            var latitude = GetGeographicValue(latitudeText);
            return new CityLocation(cityName, longitude, latitude);
        }

        private double GetGeographicValue(string text)
        {
            var degrees = int.Parse(text.Substring(0, 2));
            var minutes = int.Parse(text.Substring(3, 2));
            return degrees + (minutes / 60d);
        }
    }
}
