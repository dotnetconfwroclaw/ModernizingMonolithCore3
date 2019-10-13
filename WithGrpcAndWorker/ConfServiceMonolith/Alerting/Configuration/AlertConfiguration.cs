namespace Alerting.Configuration
{
    public class AlertConfiguration
    {
        public int CycleDelayInSeconds { get; set; }
        public string City { get; set; }
        public int LowerTemperatureThreshold { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
