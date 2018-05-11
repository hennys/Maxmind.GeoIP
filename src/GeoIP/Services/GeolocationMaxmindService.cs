using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using EPiServer.Web;
using MaxMind.GeoIP2;
using Pixie.Extensions.Maxmind.GeoIp.Models;

namespace Pixie.Extensions.Maxmind.GeoIp.Services
{
    public class GeolocationMaxmindService : IGeolocationService
    {
        private string _maxMindDatabaseFileName = "GeoLite2-City.mmdb";

        public GeoLocationResult GetGeoLocation(IPAddress address, NameValueCollection config)
        {
            string text = config["databaseFileName"];

            if (!string.IsNullOrEmpty(text))
            {
                _maxMindDatabaseFileName = VirtualPathUtilityEx.RebasePhysicalPath(text);
                config.Remove("databaseFileName");
            }

            if (string.IsNullOrWhiteSpace(_maxMindDatabaseFileName))
            {
                throw new ArgumentException("db name is not provided");
            }

            if (!System.IO.File.Exists(_maxMindDatabaseFileName))
            {
                throw new ArgumentException(string.Format("db does not exist at location {0}", _maxMindDatabaseFileName));
            }

            if (address.AddressFamily != AddressFamily.InterNetwork &&
                address.AddressFamily != AddressFamily.InterNetworkV6)
            {
                return null;
            }

            using (var reader = new DatabaseReader(_maxMindDatabaseFileName))
            {
                var dbResult = reader.City(address);

                if (dbResult == null)
                {
                    return null;
                }

                return new GeoLocationResult
                {
                    CountryCode = dbResult.Country.IsoCode,
                    CountryName = dbResult.Country.Name,
                    Latitude = dbResult.Location.Latitude ?? 0,
                    Longitude = dbResult.Location.Longitude ?? 0,
                    MetroCode = dbResult.Location.MetroCode ?? 0,
                    City = dbResult.City.Name,
                    PostalCode = dbResult.Postal.Code,
                    CountinentCode = dbResult.Continent.Code,
                    Region = dbResult?.MostSpecificSubdivision?.IsoCode,
                    RegionName = dbResult.MostSpecificSubdivision?.Name
                };
            }
        }
    }
}
