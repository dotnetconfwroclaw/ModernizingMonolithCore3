using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace AirlyPublicApiTestConsoleApp
{
    public class RequestSender
    {
        static readonly HttpClient client = new HttpClient();

        public async Task<(string response, string error, double seconds)> GetResponse(string url)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {                
                var response = await client.GetAsync(url);
                var isSuccess = response.IsSuccessStatusCode;
                var error = isSuccess ? default : response.StatusCode.ToString();  
                var responseAsString = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    error = responseAsString;
                stopwatch.Stop();
                return (responseAsString, error, stopwatch.Elapsed.TotalSeconds);
            }
            catch (HttpRequestException e)
            {
                stopwatch.Stop();
                return (null, e.Message, stopwatch.Elapsed.TotalSeconds);
            }
        }
    }
}
