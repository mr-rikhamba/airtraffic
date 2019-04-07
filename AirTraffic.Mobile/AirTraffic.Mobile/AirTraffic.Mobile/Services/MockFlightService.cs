using AirTraffic.Mobile.Helpers;
using AirTraffic.Mobile.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AirTraffic.Mobile.Services
{
    public class MockFlightService : IFlightService
    {
        public async Task<List<TimetableModel>> GetAirportFlights(string iataCode, string flightType)
        {
            var response = await ReadJsonFile<TimetableModel>("Timetables.json");
            return response;
        }

        public async Task<List<CityModel>> GetCities(string countryCode)
        {
            var response = await ReadJsonFile<CityModel>("Cities.json");
            return response;
        }

        public async Task<List<AirportModel>> GetNearByAirport(double latitude, double longitude)
        {
            var response = await ReadJsonFile<AirportModel>("Airports.json");
            return response;
        }

        private async Task<List<T>> ReadJsonFile<T>(string fileName)
        {
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(LoadResourceText)).Assembly;
                Stream stream = assembly.GetManifestResourceStream($"AirTraffic.Mobile.MockData.{fileName}");

                using (var reader = new StreamReader(stream))
                {
                    var json = await reader.ReadToEndAsync();

                    var data = JsonConvert.DeserializeObject<List<T>>(json);
                    return data;
                }
            }
            catch (System.Exception ex)
            {

                throw new System.Exception("The data service is unavailable at the moment. Please try again later.");
            }


        }
    }
}
