using Pixie.Extensions.Maxmind.GeoIp.Services;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using Xunit;

namespace GeoIpTests
{
    public class MaxmindGeoIp2Tests
    {
        private readonly string _databaseLocation;
        private readonly GeolocationMaxmindService service = new GeolocationMaxmindService();

        public MaxmindGeoIp2Tests()
        {
            _databaseLocation = Path.Combine(Environment.CurrentDirectory, @"..\..\db\GeoLite2-City.mmdb");
        }

        [Fact]
        public void MaxmindServiceDBTest()
        {
            var config = new NameValueCollection { { "databaseFileName", _databaseLocation } };
            var result = service.GetGeoLocation(IPAddress.Parse("213.205.251.152"), config);
            Assert.Equal("GB", result.CountryCode);
        }

    }
}
