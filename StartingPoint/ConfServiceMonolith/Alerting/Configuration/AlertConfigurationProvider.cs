using Microsoft.Extensions.Configuration;
using System;

namespace Alerting.Configuration
{
    public class AlertConfigurationProvider : IAlertConfigurationProvider
    {
        Lazy<AlertConfiguration> _configuration;
        public AlertConfigurationProvider(IConfiguration configuration)
        {
            _configuration = new Lazy<AlertConfiguration>(() => configuration.GetSection("AlertDetection").Get<AlertConfiguration>());
        }
        public AlertConfiguration Configuration
        {
            get
            {
                return _configuration.Value;
            }
        }
    }
}
