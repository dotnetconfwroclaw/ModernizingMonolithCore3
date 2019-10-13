namespace Alerting
{
    public class TemperatureTooLowEventArgs
    {
        public TemperatureTooLowEventArgs(string city, float temperatureThreshold, float alertTemperature)
        {
            City = city;
            TemperatureThreshold = temperatureThreshold;
            AlertTemperature = alertTemperature;
        }

        public string City { get; }
        public float TemperatureThreshold { get; }
        public float AlertTemperature { get; }

        public override string ToString()
        {
            return $"{City} alert for low temperature {AlertTemperature} being too low (lower than {TemperatureThreshold})";
        }
    }
}
