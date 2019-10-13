using Microsoft.Extensions.DependencyInjection;
using AirlyAccessing.Configuration;
using AirlyAccessing.FunctionalRequesting;
using AirlyAccessing.TechnicalRequesting;
using AirlyInterface;

namespace ConfServiceMonolithPublicApi.DI
{
    public static class AirlyServiceCollectionExtension
    {
        public static IServiceCollection AddAirly(this IServiceCollection services)
        {
            services.AddSingleton<IMeasurementsProvider, MeasurementsProvider>();
            services.AddSingleton<IAirlyConfigurationProvider, AirlyConfigurationProvider>();
            services.AddSingleton<IApiLimitChecker, ApiLimitChecker>();
            services.AddSingleton<IApiRequestExecutor, ApiRequestExecutor>();
            services.AddSingleton<ICachingApiResponseProvider, CachingApiResponseProvider>();
            return services;
        }
    }
}
