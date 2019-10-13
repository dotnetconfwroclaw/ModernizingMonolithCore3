using AirlyAccessing.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirlyAccessing.TechnicalRequesting
{
    public class CachingApiResponseProvider : ICachingApiResponseProvider
    {
        private readonly IApiLimitChecker apiLimitChecker;
        private readonly IApiRequestExecutor apiRequestExecutor;
        private readonly TimeSpan sameApiCallMaximumFrequency;
        readonly Dictionary<string, TechnicalResponse> _cachedResponses = new Dictionary<string, TechnicalResponse>();

        public CachingApiResponseProvider(IAirlyConfigurationProvider configuration, IApiLimitChecker apiLimitChecker, IApiRequestExecutor apiRequestExecutor)
        {
            sameApiCallMaximumFrequency = TimeSpan.FromSeconds(configuration.Configuration.SameApiCallMaximumFrequencyInSeconds);

            this.apiLimitChecker = apiLimitChecker;
            this.apiRequestExecutor = apiRequestExecutor;
        }

        private TechnicalResponse GetCachedResponse(string requestText)
        {
            return _cachedResponses.TryGetValue(requestText, out var cachedResponse)
                ? cachedResponse
                : null;
        }

        private void StoreResponse(TechnicalResponse response)
        {
            _cachedResponses[response.RequestText] = response;
        }

        public async Task<TechnicalResponse> GetApiResponse(string requestText, DateTime now)
        {
            var cachedResponse = GetCachedResponse(requestText);
            var stale = IsResponseStale(cachedResponse?.TimeOfRequest, now);
            if (!stale)
            {
                return cachedResponse;
            }
            return await GetNewResponseAndFallbackToCached(requestText, cachedResponse, now);
        }
        public bool IsResponseStale(DateTime? timeOfRequest, DateTime now)
        {
            if (timeOfRequest.HasValue)
                return sameApiCallMaximumFrequency < now - timeOfRequest.Value;
            return true;
        }

        private async Task<TechnicalResponse> GetNewResponseAndFallbackToCached(string requestText, TechnicalResponse cachedResponse, DateTime timeOfRequesting)
        {
            var latestResponse = await GetResponseRespectingLimits(requestText, timeOfRequesting);
            if (latestResponse != null && latestResponse.IsSuccess)
            {
                StoreResponse(latestResponse);
                return latestResponse;
            }
            return cachedResponse ?? latestResponse;            
        }

        private async Task<TechnicalResponse> GetResponseRespectingLimits(string requestText, DateTime timeOfRequesting)
        {
            if (!apiLimitChecker.CanExecuteRequest(timeOfRequesting))
            {
                return null;
            }

            var response = await apiRequestExecutor.GetRawResponse(requestText, timeOfRequesting);
            apiLimitChecker.SetResultingApiLimits(timeOfRequesting, response.QueriesLeftThisMinute, response.QueriesLeftToday);
            return response;
        }
    }
}
