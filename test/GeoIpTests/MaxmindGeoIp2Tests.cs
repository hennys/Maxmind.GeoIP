using Pixie.Extensions.Maxmind.GeoIp.Services;
using System.Collections.Specialized;
using System.Net;
using Xunit;

namespace GeoIpTests
{
    public class MaxmindGeoIp2Tests
    {
        private GeolocationMaxmindService service = new GeolocationMaxmindService();

        [Fact]
        public void MaxmindServiceDBTest()
        {
            NameValueCollection config = new NameValueCollection();
            config.Add("databaseFileName", @"C:\Pixie\Research\EPiServer\Maxmind.GeoIP\GeoIpTests\db\GeoLite2-City.mmdb");
            var result = service.GetGeoLocation(IPAddress.Parse("213.205.251.152"), config);
            Assert.Equal("GB", result.CountryCode);
        }

    }
}
