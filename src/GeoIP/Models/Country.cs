using System;

namespace Pixie.Extensions.Maxmind.GeoIp.Models
{
    public class Country
    {
        public Country(string code, string name)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));
            if (code.Length != 2) throw new ArgumentException("Invalid country code, should be a two-letter code", nameof(code));

            Code = code;
            Name = name;
        }

        public string Code { get; }

        public string Name { get; }
    }
}
