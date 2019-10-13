using Alerting;
using Alerting.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConfServiceMonolithPublicApi.DI
{
    public static class AlertingServicesCollectionExtension
    {
        public static IServiceCollection AddAlerting(this IServiceCollection services)
        {
            services.AddSingleton<IAlertChecker, AlertChecker>();
            services.AddSingleton<IAlertConfigurationProvider, AlertConfigurationProvider>();
            return services;
        }
    }
}
