using System.Collections.Specialized;
using System.Net;
using Pixie.Extensions.Maxmind.GeoIp.Models;

namespace Pixie.Extensions.Maxmind.GeoIp.Services
{
    public interface IGeolocationService
    {
        GeoLocationResult GetGeoLocation(IPAddress address, NameValueCollection config);
    }
}
