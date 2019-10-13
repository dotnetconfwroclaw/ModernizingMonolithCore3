using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirlyCache;
using AirlyInterface;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService
{
    public class GreeterService : AirlyCache.AirlyGrpc.AirlyGrpcBase
    {
        private readonly ILogger<GreeterService> logger;
        private readonly IMeasurementsProvider measurementsProvider;

        public GreeterService(ILogger<GreeterService> logger, IMeasurementsProvider measurementsProvider)
        {
            this.logger = logger;
            this.measurementsProvider = measurementsProvider;
        }

        public async override Task<AirlyResponse> GetMeasurementsByLocation(AirlyRequest request, ServerCallContext context)
        {

            var apiResponse = await measurementsProvider.GetMeasurementsByLocation(request.Latitude, request.Longitude);
            var result = new AirlyResponse
            {
                ErrorText = apiResponse.ErrorText,
                FromDateTimeTicks = apiResponse.FromDateTime.Ticks,
                StatusCode = (ResponseStatus)apiResponse.StatusCode
            };

            result.Values.AddRange(context.RequestHeaders.Select(h => new ValueDescriptor { Name = h.Key + ": " + h.Value, Value = 1 }));

            result.Values.AddRange(apiResponse.Values.Select(x => new ValueDescriptor { Name = x.Name, Value = x.Value }));
            return result;
        }
    }
}
