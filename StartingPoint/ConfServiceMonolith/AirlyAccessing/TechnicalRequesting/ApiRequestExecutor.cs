using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AirlyAccessing.Configuration;
using Flurl;

namespace AirlyAccessing.TechnicalRequesting
{
    public class ApiRequestExecutor : IApiRequestExecutor
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IAirlyConfigurationProvider configurationProvider;
        private const string HttpClientName = "HttpAirlyClient";

        public ApiRequestExecutor(IHttpClientFactory httpClientFactory, IAirlyConfigurationProvider configurationProvider)
        {
            this.httpClientFactory = httpClientFactory;
            this.configurationProvider = configurationProvider;
        }

        public async Task<TechnicalResponse> GetRawResponse(string requestText, DateTime timeOfRequest)
        {
            var request = Url.Combine(configurationProvider.Configuration.Url, requestText);
            var httpClient = httpClientFactory.CreateClient(HttpClientName);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, request);
            httpRequest.Headers.Add("apikey", configurationProvider.Configuration.ApiKey);
            httpRequest.Headers.Add("Accept", "application/json");
            httpRequest.Headers.Add("Accept-Language", "pl");

            var httpResponseMessage = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseContentRead);       
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            var queriesLeftThisMinute = GetIntFromTextIfCanBeParsed(httpResponseMessage.Headers.GetValues("X-RateLimit-Remaining-minute"));
            var queriesLeftThisDay = GetIntFromTextIfCanBeParsed(httpResponseMessage.Headers.GetValues("X-RateLimit-Remaining-day"));
            return new TechnicalResponse(
                isSuccess: httpResponseMessage.IsSuccessStatusCode,
                responseText: responseString,
                statusCode: httpResponseMessage.StatusCode,
                queriesLeftToday: queriesLeftThisDay,
                queriesLeftThisMinute: queriesLeftThisMinute,
                requestText: requestText,
                timeOfRequest: timeOfRequest
                );
        }

        private int GetIntFromTextIfCanBeParsed(IEnumerable<string> values)
        {
            return int.TryParse(values.FirstOrDefault(), out var result)
                ? result
                : 1;// Still allow querying if we can't parse response.
        }
    }
}
