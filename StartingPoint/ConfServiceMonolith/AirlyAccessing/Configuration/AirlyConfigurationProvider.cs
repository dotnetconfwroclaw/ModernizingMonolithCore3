using System;
using Microsoft.Extensions.Configuration;

namespace AirlyAccessing.Configuration
{
    public class AirlyConfigurationProvider : IAirlyConfigurationProvider
    {
        Lazy<AccessConfiguration> _configuration;
        public AirlyConfigurationProvider(IConfiguration configuration)
        {
            _configuration = new Lazy<AccessConfiguration>(() => configuration.GetSection("AirlyApi").Get<AccessConfiguration>());
        }
        public AccessConfiguration Configuration
        {
            get
            {
                return _configuration.Value;
            }
        }
    }
}
