using AirlyInterface.Domain;
using System.Threading.Tasks;

namespace AirlyInterface
{
    public interface IMeasurementsProvider
    {
        Task<AirQualityResponse> GetMeasurementsByLocation(double latitude, double longitude);
    }
}