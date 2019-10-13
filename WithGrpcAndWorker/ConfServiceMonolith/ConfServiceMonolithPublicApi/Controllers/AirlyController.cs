using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using AirlyInterface;

namespace ConfServiceMonolithPublicApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AirlyController : ControllerBase
    {
        private readonly IMeasurementsProvider measurementsProvider;

        public AirlyController(IMeasurementsProvider measurementsProvider)
        {
            this.measurementsProvider = measurementsProvider;
        }

        [HttpGet]
        public string Get()
        {
            return "Default endpoint";
        }

        //https://localhost:5001/airly/measurements?lat=50.062006&lng=19.940984
        [HttpGet("measurements")]
        public async Task<IActionResult> GetMeasurements(string lat, string lng)
        {            
            var culture = CultureInfo.InvariantCulture;
            var longitude = double.Parse(lng, culture);
            var latitude = double.Parse(lat, culture);
            var response = await measurementsProvider.GetMeasurementsByLocation(latitude: latitude, longitude: longitude);
            if (response.StatusCode == AirlyInterface.Domain.AirlyStatusCode.Ok)
                return Ok(response.Values);
            else return NotFound(response.ErrorText);           
        }
    }
}

