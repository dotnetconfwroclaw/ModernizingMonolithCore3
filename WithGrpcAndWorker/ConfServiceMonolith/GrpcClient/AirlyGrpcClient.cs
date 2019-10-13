using System;
using System.Linq;
using System.Threading.Tasks;
using AirlyCache;
using AirlyInterface;
using AirlyInterface.Domain;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

namespace GrpcClient
{
    public class AirlyGrpcClient : IMeasurementsProvider
    {

        private readonly ILoggerFactory loggerFactory;

        public AirlyGrpcClient(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }
        public async Task<AirQualityResponse> GetMeasurementsByLocation(double latitude, double longitude)
        {

            var channelOptions = new GrpcChannelOptions { LoggerFactory = loggerFactory };
            var channel = GrpcChannel.ForAddress("https://localhost:5001", channelOptions);

            var client = new AirlyGrpc.AirlyGrpcClient(channel);
            var headers = new Metadata();
            var callOptions = new CallOptions().WithHeaders(headers);
            headers.Add("Correlation-id", Guid.NewGuid().ToString());
            var grpcResponse = await client.GetMeasurementsByLocationAsync(
                new AirlyRequest { Latitude = latitude, Longitude = longitude }, callOptions);
            return new AirQualityResponse((AirlyStatusCode)grpcResponse.StatusCode,
                grpcResponse.ErrorText,
                new DateTime(grpcResponse.FromDateTimeTicks, DateTimeKind.Utc),
                grpcResponse.Values.Select(x => new AirlyNamedValue(x.Name, x.Value)));

        }
    }
}
