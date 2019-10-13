using AirlyInterface;
using AirlyInterface.Domain;
using Alerting.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;

namespace Alerting
{
    public class AlertChecker : IAlertChecker
    {
        private readonly ILogger<IAlertChecker> logger;
        private readonly IMeasurementsProvider measurementsProvider;
        private readonly IAlertConfigurationProvider alertConfigurationProvider;

        public event Action<TemperatureTooLowEventArgs> OnTemperatureTooLowAlert;
        public AlertChecker(ILogger<IAlertChecker> logger, IMeasurementsProvider measurementsProvider, IAlertConfigurationProvider alertConfigurationProvider)
        {
            this.logger = logger;
            this.measurementsProvider = measurementsProvider;
            this.alertConfigurationProvider = alertConfigurationProvider;
        }
        public void CheckAndRaiseAlertIfNeeded(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            var eventArgs = CheckIfAlertShouldBeRaised();

            if (eventArgs != null)
            {
                RaiseAlert(eventArgs);
            }           
        }        

        private void RaiseAlert(TemperatureTooLowEventArgs eventArgs)
        {
            var onTemperatureTooLowAlert = OnTemperatureTooLowAlert;
            if (onTemperatureTooLowAlert != null)
            {
                logger.LogInformation($"Raising alert {eventArgs}.");
                onTemperatureTooLowAlert(eventArgs);
            }
        }

        private TemperatureTooLowEventArgs CheckIfAlertShouldBeRaised()
        {
            var temperatureThreshold = alertConfigurationProvider.Configuration.LowerTemperatureThreshold;
            var city = alertConfigurationProvider.Configuration.City;

            logger.LogInformation($"Checking alert condition if temperature of {temperatureThreshold} is too low in {city}.");

            return GetEventArgsIfShouldRaiseAlert(
                longitude: alertConfigurationProvider.Configuration.Longitude,
                latitude: alertConfigurationProvider.Configuration.Latitude,
                temperatureThreshold: temperatureThreshold,
                city: city,
                now: DateTime.Now);
        }

        private TemperatureTooLowEventArgs GetEventArgsIfShouldRaiseAlert(double longitude, double latitude, float temperatureThreshold, string city, DateTime now)
        {
            var airQualityResponse = measurementsProvider.GetMeasurementsByLocation(longitude: longitude, latitude: latitude).Result;
            if (airQualityResponse.StatusCode == AirlyStatusCode.Ok)
            {
                var temperatureMeasurement = airQualityResponse.Values
                    .Where(x => x.Name == "Temperature")
                    .FirstOrDefault();

                if (temperatureMeasurement != null && temperatureMeasurement.Value < temperatureThreshold)
                {
                    return new TemperatureTooLowEventArgs(city, temperatureThreshold, (float)temperatureMeasurement.Value);
                }
            }
            return null;
        }
    }
}
