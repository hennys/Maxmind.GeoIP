using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using EPiServer.Personalization;
using Pixie.Extensions.Maxmind.GeoIp.Models;
using Pixie.Extensions.Maxmind.GeoIp.Services;

namespace Pixie.Extensions.Maxmind.GeoIp.Provider
{
    public class MaxmindGeoIp2 : GeolocationProviderBase
    {
        private readonly IGeolocationService _geolocationService;
        private readonly NameValueCollection _baseConfig = new NameValueCollection();
        private readonly NameValueCollection _extraConfig = new NameValueCollection();
        private Capabilities _capabilities;

        public MaxmindGeoIp2() : this(new GeolocationMaxmindService()) { }

        public MaxmindGeoIp2(IGeolocationService geolocationService)
        {
            _geolocationService = geolocationService;
        }

        public override Capabilities Capabilities => _capabilities;

        public override void Initialize(string name, NameValueCollection config)
        {
            for (int i = 0; i < config.Count; i++)
            {
                string key = config.GetKey(i);
                switch (key)
                {
                    case "name":
                    case "type":
                        _baseConfig.Add(key, config[i]);
                        break;
                    default:
                        _extraConfig.Add(key, config[i]);
                        break;
                }
            }

            _capabilities = (Capabilities.Location | Capabilities.ContinentCode | Capabilities.CountryCode | Capabilities.Region);

            base.Initialize(name, _baseConfig);
        }

        public override IEnumerable<string> GetContinentCodes() 
            => Geographics.Continents.Keys;

        public override IEnumerable<string> GetCountryCodes(string continentCode)
        {
            string foundContinentCode;
            IEnumerable<Country> countries = from country in Geographics.Countries
                                             where !country.Code.Equals("--", StringComparison.Ordinal) && !country.Code.Equals("EU", StringComparison.Ordinal) && !country.Code.Equals("AP", StringComparison.Ordinal) && Geographics.CountryToContinent.TryGetValue(country.Code, out foundContinentCode) && foundContinentCode.Equals(continentCode, StringComparison.OrdinalIgnoreCase)
                                             select country;

            return countries.Select(x => x.Code);
        }

        public override IEnumerable<string> GetRegions(string countryCode)
        {
            if (countryCode == null) throw new ArgumentNullException(nameof(countryCode));

            if (Geographics.Regions.TryGetValue(countryCode, out var dictionary))
            {
                return dictionary.Values;
            }
            return Enumerable.Empty<string>();
        }

        public override IGeolocationResult Lookup(IPAddress address) 
            => _geolocationService.GetGeoLocation(address, _extraConfig);
    }
}
