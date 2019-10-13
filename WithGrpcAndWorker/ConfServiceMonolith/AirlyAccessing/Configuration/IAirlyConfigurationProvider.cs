namespace AirlyAccessing.Configuration
{
    public interface IAirlyConfigurationProvider
    {
        AccessConfiguration Configuration { get; }
    }
}