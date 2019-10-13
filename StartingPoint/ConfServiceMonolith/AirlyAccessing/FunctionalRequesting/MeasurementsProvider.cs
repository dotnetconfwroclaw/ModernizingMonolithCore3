using AirlyAccessing.FunctionalRequesting.JsonModel;
using AirlyAccessing.TechnicalRequesting;
using AirlyInterface;
using AirlyInterface.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AirlyAccessing.FunctionalRequesting
{
    public class MeasurementsProvider : IMeasurementsProvider
    {
        private readonly ICachingApiResponseProvider apiResponseProvider;

        public MeasurementsProvider(ICachingApiResponseProvider apiResponseProvider)
        {
            this.apiResponseProvider = apiResponseProvider;
        }

        public async Task<AirQualityResponse> GetMeasurementsByLocation(double latitude, double longitude)
        {
            var latitudeString = latitude.ToString("F6", CultureInfo.InvariantCulture);
            var longitudeString = longitude.ToString("F6", CultureInfo.InvariantCulture);
            var request = $"measurements/nearest?lat={latitudeString}&lng={longitudeString}&maxDistanceKM=10";

            var response = await apiResponseProvider.GetApiResponse(request, DateTime.Now);
            if (response == null)
            {
                return new AirQualityResponse(
                    statusCode: AirlyStatusCode.OtherError,
                    errorText: "Can't get the response, API Limit exceeded and no cached results for such request",
                    fromDateTime: DateTime.UtcNow, 
                    values: new List<AirlyNamedValue>()
                    );
            }

            AirQualityDescription modelObject = ConvertToModelObject(response);
            var fromDateTime = modelObject.current != null ? modelObject.current.fromDateTime : default;
            var values = modelObject.current != null
                ? modelObject.current.values.Select(x => new AirlyNamedValue(x.name, x.value)).ToList()
                : new List<AirlyNamedValue>();

            return new AirQualityResponse(
                statusCode: TranslateStatusCode(response.StatusCode),
                errorText: response.ResponseText,
                fromDateTime: fromDateTime,
                values: values
                );
        }

        private static AirQualityDescription ConvertToModelObject(TechnicalResponse response)
        {
            AirQualityDescription modelObject = null;
            try
            {
                modelObject = JsonConvert.DeserializeObject<AirQualityDescription>(response.ResponseText);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception deserializing json: {e}");
                Console.WriteLine($"Problematic Json: {response.RequestText}");
            }

            return modelObject;
        }

        private AirlyStatusCode TranslateStatusCode(HttpStatusCode httpStatusCode)
        {
            switch (httpStatusCode)
            {
                case HttpStatusCode.OK: return AirlyStatusCode.Ok;
                case HttpStatusCode.NotFound: return AirlyStatusCode.NotFound;
                default:
                    return AirlyStatusCode.OtherError;
            }
        }

    }
}
