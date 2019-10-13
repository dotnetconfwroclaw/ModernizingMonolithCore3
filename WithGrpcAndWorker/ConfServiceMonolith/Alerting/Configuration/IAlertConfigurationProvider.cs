namespace Alerting.Configuration
{
    public interface IAlertConfigurationProvider
    {
        AlertConfiguration Configuration { get; }
    }
}
