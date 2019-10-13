using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AirlyPublicApiTestConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var polishCitiesText = File.ReadAllLines("polishcities.txt");
            var polishCities = new CityReader().GetCityLocations(polishCitiesText).ToDictionary(x => x.CityName, x => x);
            var resultsDictionary = new Dictionary<string, QueryResult>();
            var random = new Random();
            var requestSender = new RequestSender();
            var cityiesList = polishCities.Values.ToList();

            while (true)
            {
                for (var i = 0; i < 100; ++i)
                {
                    var city = cityiesList[random.Next(cityiesList.Count)];
                    var alreadyHaveResponse = resultsDictionary.TryGetValue(city.CityName, out var receivedResult);

                    var url = $"https://localhost:5005/airly/measurements?lat={city.LatitudeText}&lng={city.LongitudeText}";
                    var (response, error, seconds) = await requestSender.GetResponse(url);
                    var infoToPrint = new QueryResult(city: city.CityName, response: response, error: error, durationInSeconds: seconds);
                    resultsDictionary[city.CityName] = infoToPrint;

                    if (alreadyHaveResponse)
                    {
                        Console.WriteLine($"Query {i + 1}, have cities: {resultsDictionary.Count}/{cityiesList.Count} {infoToPrint.ToString(31)}<-refresh!");
                    }
                    else
                    {
                        Console.WriteLine($"Query {i + 1}, have cities: {resultsDictionary.Count}/{cityiesList.Count} {infoToPrint}");
                    }

                }
                Console.WriteLine("Press enter to load another 100...");
                Console.ReadLine();
            }
        }
    }
}
